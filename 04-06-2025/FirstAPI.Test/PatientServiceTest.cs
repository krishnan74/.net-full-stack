using FirstAPI.Contexts;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.Patient;
using FirstAPI.Services;
using FirstAPI.Interfaces;
using Moq;
using AutoMapper;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FirstAPI.Test
{
    public class PatientServiceTest
    {
        private Mock<IRepository<int, Patient>> _patientRepositoryMock;
        private Mock<IRepository<string, User>> _userRepositoryMock;
        private Mock<IEncryptionService> _encryptionServiceMock;
        private Mock<IMapper> _mapperMock;
        private PatientService _patientService;

        [SetUp]
        public void Setup()
        {
            _patientRepositoryMock = new Mock<IRepository<int, Patient>>();
            _userRepositoryMock = new Mock<IRepository<string, User>>();
            _encryptionServiceMock = new Mock<IEncryptionService>();
            _mapperMock = new Mock<IMapper>();
            _patientService = new PatientService(
                _patientRepositoryMock.Object,
                _userRepositoryMock.Object,
                _encryptionServiceMock.Object,
                _mapperMock.Object
            );
        }

        [Test]
        public async Task AddPatient_Test()
        {
            var dto = new PatientAddRequestDTO { Name = "Test", Diagnosis = "Flu", Email = "test@email.com", Password = "pass" };
            var user = new User { Username = dto.Email, Password = new byte[] { 1, 2 }, HashKey = new byte[] { 3, 4 }, Role = "Patient" };
            var patient = new Patient { Name = dto.Name, Diagnosis = dto.Diagnosis, Email = dto.Email };

            _mapperMock.Setup(m => m.Map<PatientAddRequestDTO, User>(dto)).Returns(user);
            _encryptionServiceMock.Setup(e => e.EncryptData(It.IsAny<EncryptModel>()))
                                    .ReturnsAsync(new EncryptModel { EncryptedData = new byte[] { 1, 2 }, HashKey = new byte[] { 3, 4 } });
            _userRepositoryMock.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync(user);
            _patientRepositoryMock.Setup(r => r.Add(It.IsAny<Patient>())).ReturnsAsync(patient);

            var result = await _patientService.AddPatient(dto);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(dto.Name));
        }

        [Test]
        public void GetPatientByName_Test_NullOrEmptyException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _patientService.GetPatientByName(null));
            Assert.ThrowsAsync<ArgumentException>(() => _patientService.GetPatientByName(" "));
        }

        [Test]
        public async Task GetPatientByName_Test()
        {
            var name = "Test";
            var patients = new List<Patient> { new Patient { Name = name } };
            _patientRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(patients);
            var result = await _patientService.GetPatientByName(name);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        public async Task GetPatientsByDiagnosis_Test()
        {
            var diagnosis = "Flu";
            var patients = new List<Patient> { new Patient { Name = "Test", Diagnosis = diagnosis } };
            var dtos = new List<PatientsByDiagnosisResponseDTO> { new PatientsByDiagnosisResponseDTO { Name = "Test", Diagnosis = diagnosis } };


            _patientRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(patients);
            _mapperMock.Setup(m => m.Map<IEnumerable<Patient>, ICollection<PatientsByDiagnosisResponseDTO>>(It.IsAny<IEnumerable<Patient>>())).Returns(dtos);
            var result = await _patientService.GetPatientsByDiagnosis(diagnosis);


            Assert.That(result, Is.Not.Null);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Diagnosis, Is.EqualTo(diagnosis));
        }

        [Test]
        public void GetPatientsByDiagnosis_Test_NullOrEmptyException()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _patientService.GetPatientsByDiagnosis(null));
            Assert.ThrowsAsync<ArgumentException>(() => _patientService.GetPatientsByDiagnosis(" "));
        }
    }
} 