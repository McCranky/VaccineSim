using SimulationLib.Generators;
using SimulationLib.Simulations.EventSimulation.Vaccination.Events;
using SimulationLib.Simulations.EventSimulation.Vaccination.Models;
using SimulationLib.Simulations.EventSimulation.Vaccination.Observables;
using SimulationLib.Statistics;
using System;
using System.Collections.Generic;

namespace SimulationLib.Simulations.EventSimulation.Vaccination
{
    public class VaccinationSimulation : EventSimulationCore
    {
        public VaccinationSettings VaccinSettings { get; protected set; }

        #region Generators
        public ExponentialGenerator ExaminationGen { get; protected set; }
        public TriangularGenerator VaccinationGen { get; protected set; }
        public UniformGenerator RegistrationGen { get; protected set; }
        public Random WaitingGen { get; protected set; }
        public Random PatientsGen { get; protected set; }
        public Random ArrivalGen { get; protected set; }
        #endregion

        #region Workplaces
        public Workplace RegistrationRoom { get; protected set; }
        public Workplace ExaminationRoom { get; protected set; }
        public Workplace VaccinationRoom { get; protected set; }
        #endregion

        #region Fronts
        public Queue<Patient> RegistrationFront { get; protected set; }
        public Queue<Patient> ExaminationFront { get; protected set; }
        public Queue<Patient> VaccinationFront { get; protected set; }
        public int WaitingRoom { get; set; }
        #endregion

        #region SingleRepplicationStatistics
        public DiscreetStatistic RegistrationWaiting { get; private set; }
        public DiscreetStatistic ExaminationWaiting { get; private set; }
        public DiscreetStatistic VaccinationWaiting { get; private set; }

        public ContinuousStatistic RegistrationLength { get; private set; }
        public ContinuousStatistic ExaminationLength { get; private set; }
        public ContinuousStatistic VaccinationLength { get; private set; }
        public ContinuousStatistic WaitingRoomLength { get; private set; }
        #endregion

        #region OverReplicationStatistics
        private DiscreetStatistic OR_RegistrationLength;
        private DiscreetStatistic OR_RegistrationWaiting;
        private DiscreetStatistic OR_RegistrationWorkload;
        private DiscreetStatistic OR_ExaminationLength;
        private DiscreetStatistic OR_ExaminationWaiting;
        private DiscreetStatistic OR_ExaminationWorkload;
        private DiscreetStatistic OR_VaccinationLength;
        private DiscreetStatistic OR_VaccinationWaiting;
        private DiscreetStatistic OR_VaccinationWorkload;
        private DiscreetStatistic OR_WaitingRoomLength;

        private DiscreetStatistic OR_MissingPatients;
        private DiscreetStatistic OR_Overtime;
        #endregion

        #region Observables
        private readonly SimulationObservable<AfterEventValues> _afterEventObservables;
        private readonly SimulationObservable<AfterReplicationValues> _afterReplicationObservables;
        private readonly SimulationObservable<DoctorsExperimentValues> _experimentObservables;
        #endregion


        public double NotArriveProbability { get; protected set; }
        public int NotArrivedPatients { get; set; }
        private int _currentDoctorsCount;

        public VaccinationSimulation(MonteCarloSettings monteCarloSettings, EventSimulationSettings eventSimulationSettings, VaccinationSettings vaccinationSettings)
            : base(monteCarloSettings, eventSimulationSettings)
        {
            VaccinSettings = vaccinationSettings;

            _afterEventObservables = new SimulationObservable<AfterEventValues>();
            _afterReplicationObservables = new SimulationObservable<AfterReplicationValues>();
            _experimentObservables = new SimulationObservable<DoctorsExperimentValues>();

            ExaminationGen = new ExponentialGenerator(1d / 260);
            VaccinationGen = new TriangularGenerator(20, 100, 75);
            RegistrationGen = new UniformGenerator(140, 220);

            WaitingGen = new Random();
            PatientsGen = new Random();
            ArrivalGen = new Random();

            if (VaccinSettings.DoctorsExperimentEnabled)
            {
                MCSettings.Replications = (VaccinSettings.DoctorsMax - VaccinSettings.DoctorsMin + 1) * VaccinSettings.ExperimentReplications;
                _currentDoctorsCount = VaccinSettings.DoctorsMin;
            }

            Init();
        }

        private void Init()
        {
            SetNotArriveProbability();

            RegistrationRoom = new Workplace(VaccinSettings.RegistrationWorkers);
            if (VaccinSettings.DoctorsExperimentEnabled)
            {
                ExaminationRoom = new Workplace(_currentDoctorsCount);
            }
            else
            {
                ExaminationRoom = new Workplace(VaccinSettings.ExaminationWorkers);
            }
            VaccinationRoom = new Workplace(VaccinSettings.VaccinationWorkers);

            RegistrationFront = new Queue<Patient>();
            ExaminationFront = new Queue<Patient>();
            VaccinationFront = new Queue<Patient>();

            RegistrationWaiting = new DiscreetStatistic();
            ExaminationWaiting = new DiscreetStatistic();
            VaccinationWaiting = new DiscreetStatistic();

            RegistrationLength = new ContinuousStatistic();
            ExaminationLength = new ContinuousStatistic();
            VaccinationLength = new ContinuousStatistic();
            WaitingRoomLength = new ContinuousStatistic();

            OR_RegistrationLength = new DiscreetStatistic();
            OR_RegistrationWaiting = new DiscreetStatistic();
            OR_RegistrationWorkload = new DiscreetStatistic();
            OR_ExaminationLength = new DiscreetStatistic();
            OR_ExaminationWaiting = new DiscreetStatistic();
            OR_ExaminationWorkload = new DiscreetStatistic();
            OR_VaccinationLength = new DiscreetStatistic();
            OR_VaccinationWaiting = new DiscreetStatistic();
            OR_VaccinationWorkload = new DiscreetStatistic();
            OR_WaitingRoomLength = new DiscreetStatistic();
            OR_MissingPatients = new DiscreetStatistic();
            OR_Overtime = new DiscreetStatistic();

            AddFirstEvent();
        }

        public override void Restart(bool overReplication = false)
        {
            base.Restart();
            Patient.ResetId();
            Worker.ResetId();
            NotArrivedPatients = 0;
            WaitingRoom = 0;
            ActualSimulationTime = 0;

            RegistrationRoom.Restart();
            ExaminationRoom.Restart();
            VaccinationRoom.Restart();

            RegistrationFront.Clear();
            ExaminationFront.Clear();
            VaccinationFront.Clear();

            SetNotArriveProbability();
            AddFirstEvent();

            RegistrationWaiting.Clear();
            ExaminationWaiting.Clear();
            VaccinationWaiting.Clear();

            RegistrationLength.Clear();
            ExaminationLength.Clear();
            VaccinationLength.Clear();
            WaitingRoomLength.Clear();

            if (!overReplication)
            {
                OR_RegistrationLength.Clear();
                OR_RegistrationWaiting.Clear();
                OR_RegistrationWorkload.Clear();
                OR_ExaminationLength.Clear();
                OR_ExaminationWaiting.Clear();
                OR_ExaminationWorkload.Clear();
                OR_VaccinationLength.Clear();
                OR_VaccinationWaiting.Clear();
                OR_VaccinationWorkload.Clear();
                OR_WaitingRoomLength.Clear();
                OR_MissingPatients.Clear();
                OR_Overtime.Clear();
            }
        }

        private void SetNotArriveProbability()
        {
            var notComming = PatientsGen.Next(VaccinSettings.NotCommingLowerBoundry, VaccinSettings.NotCommingHigherBoundry);
            NotArriveProbability = notComming / (double)VaccinSettings.Patients;
        }

        private void AddFirstEvent()
        {
            Timeline.Enqueue(new PatientArrival
            {
                Simulation = this,
                Time = ActualSimulationTime,
                Patient = new Patient()
            });
        }

        protected override void AfterEvent()
        {
            if (Settings.IsWatching)
            {
                var guiValues = GetAfterEventValues();
                _afterEventObservables.Next(guiValues);
            }
        }

        protected override void AfterReplication()
        {
            OR_RegistrationLength.Add(RegistrationLength.Average);
            OR_RegistrationWaiting.Add(RegistrationWaiting.Average);
            OR_RegistrationWorkload.Add(RegistrationRoom.Workload);
            OR_ExaminationLength.Add(ExaminationLength.Average);
            OR_ExaminationWaiting.Add(ExaminationWaiting.Average);
            OR_ExaminationWorkload.Add(ExaminationRoom.Workload);
            OR_VaccinationLength.Add(VaccinationLength.Average);
            OR_VaccinationWaiting.Add(VaccinationWaiting.Average);
            OR_VaccinationWorkload.Add(VaccinationRoom.Workload);
            OR_WaitingRoomLength.Add(WaitingRoomLength.Average);
            OR_MissingPatients.Add(NotArrivedPatients);
            OR_Overtime.Add(ActualSimulationTime - Settings.SimulationTime);

            var afterRep = GetAfterReplicationValues();
            _afterReplicationObservables.Next(afterRep);

            var afterEvent = GetAfterEventValues();
            _afterEventObservables.Next(afterEvent);

            // ak je pusteny experiment a je dosiahnuta hranična replikacia
            if (VaccinSettings.DoctorsExperimentEnabled
                && ActualReplication % VaccinSettings.ExperimentReplications == 0)
            {
                // poslanie výsledkov experimentu
                var results = new DoctorsExperimentValues
                {
                    DoctorsCount = _currentDoctorsCount,
                    AverageFrontLength = OR_ExaminationLength.Average,
                    AverageFrontWaiting = OR_ExaminationWaiting.Average
                };
                _experimentObservables.Next(results);

                // pokračovanie v experimente ak je to možne
                if (_currentDoctorsCount < VaccinSettings.DoctorsMax)
                {
                    OR_ExaminationLength.Clear();
                    OR_ExaminationWaiting.Clear();

                    ++_currentDoctorsCount;
                    ExaminationRoom = new Workplace(_currentDoctorsCount);
                }
            }

            Restart(true);
        }

        public IDisposable Subscribe(IObserver<AfterEventValues> observer)
        {
            var unsubscriber = _afterEventObservables.Subscribe(observer);
            observer.OnNext(GetAfterEventValues());
            return unsubscriber;
        }
        public IDisposable Subscribe(IObserver<AfterReplicationValues> observer)
        {
            var unsubscriber = _afterReplicationObservables.Subscribe(observer);
            observer.OnNext(GetAfterReplicationValues());
            return unsubscriber;
        }
        public IDisposable Subscribe(IObserver<DoctorsExperimentValues> observer)
        {
            var unsubscriber = _experimentObservables.Subscribe(observer);
            return unsubscriber;
        }

        private AfterEventValues GetAfterEventValues()
        {
            return new AfterEventValues
            {
                SimulationTime = ActualSimulationTime,
                RegistrationWorkersStats = RegistrationRoom.GetWorkersStats(),
                RegistrationFrontLength = RegistrationFront.Count,
                RegistrationAverageTime = RegistrationWaiting.Average,
                RegistrationAverageLength = RegistrationLength.Average,
                ExaminationWorkersStats = ExaminationRoom.GetWorkersStats(),
                ExaminationFrontLength = ExaminationFront.Count,
                ExaminationAverageTime = ExaminationWaiting.Average,
                ExaminationAverageLength = ExaminationLength.Average,
                VaccinationWorkersStats = VaccinationRoom.GetWorkersStats(),
                VaccinationFrontLength = VaccinationFront.Count,
                VaccinationAverageTime = VaccinationWaiting.Average,
                VaccinationAverageLength = VaccinationLength.Average,
                WaitingRoomPatients = WaitingRoom,
                WaitingRoomAverageLength = WaitingRoomLength.Average
            };
        }

        private AfterReplicationValues GetAfterReplicationValues()
        {
            return new AfterReplicationValues
            {
                Replication = ActualReplication,
                RegistrationAverageLength = OR_RegistrationLength.Average,
                RegistrationAverageTime = OR_RegistrationWaiting.Average,
                RegistrationWorkload = OR_RegistrationWorkload.Average,
                ExaminationAverageLength = OR_ExaminationLength.Average,
                ExaminationAverageTime = OR_ExaminationWaiting.Average,
                ExaminationWorkload = OR_ExaminationWorkload.Average,
                VaccinationAverageLength = OR_VaccinationLength.Average,
                VaccinationAverageTime = OR_VaccinationWaiting.Average,
                VaccinationWorkload = OR_VaccinationWorkload.Average,
                WaitingRoomLengthStats = OR_WaitingRoomLength,
                MissingPatients = OR_MissingPatients.Average,
                Overtime = OR_Overtime.Average
            };
        }
    }
}
