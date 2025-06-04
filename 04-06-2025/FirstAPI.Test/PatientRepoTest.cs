using FirstAPI.Contexts;
using FirstAPI.Models;
using FirstAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace FirstAPI.Test
{
    public class PatientRepoTest
    {
        private ClinicContext _context;
        private PatientRepository _patientRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ClinicContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Patient")
                .Options;
            _context = new ClinicContext(options);
            _patientRepository = new PatientRepository(_context);
        }

        [Test]
        public async Task AddPatient_Test()
        {
            var patient = new Patient { Name = "victor", Diagnosis = "Flu", Email = "victor@email.com" };
            var result = await _patientRepository.Add(patient);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.GreaterThan(0));
        }

        [Test]
        public async Task GetPatient_Test()
        {
            var patient = new Patient { Name = "Test", Diagnosis = "Flu", Email = "test@email.com" };
            await _patientRepository.Add(patient);
            var result = await _patientRepository.Get(patient.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Test"));
        }

        [Test]
        public void GetPatient_Test_NotFoundException()
        {
            Assert.ThrowsAsync<System.Exception>(async () => await _patientRepository.Get(999));
        }

        [Test]
        public async Task GetAllPatients_Test()
        {
            await _patientRepository.Add(new Patient { Name = "potter", Diagnosis = "Flu", Email = "potter@email.com" });
            await _patientRepository.Add(new Patient { Name = "harry", Diagnosis = "Cold", Email = "harry@email.com" });
            var result = await _patientRepository.GetAll();
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetAllPatients_Test_EmptyException()
        {
            Assert.ThrowsAsync<System.Exception>(async () => await _patientRepository.GetAll());
        }

        [Test]
        public async Task UpdatePatient_Test()
        {
            var patient = new Patient { Name = "Test", Diagnosis = "Flu", Email = "test@email.com" };
            await _patientRepository.Add(patient);
            patient.Name = "Updated";
            var result = await _patientRepository.Update(patient.Id, patient);
            Assert.That(result.Name, Is.EqualTo("Updated"));
        }

        [Test]
        public void UpdatePatient_Test_NotFoundException()
        {
            var patient = new Patient { Id = 999, Name = "NotFound" };
            Assert.ThrowsAsync<System.Exception>(async () => await _patientRepository.Update(patient.Id, patient));
        }

        [Test]
        public async Task DeletePatient_Test()
        {
            var patient = new Patient { Name = "Test", Diagnosis = "Flu", Email = "test@email.com" };
            await _patientRepository.Add(patient);
            var result = await _patientRepository.Delete(patient.Id);
            Assert.That(result, Is.Not.Null);
            Assert.ThrowsAsync<System.Exception>(async () => await _patientRepository.Get(patient.Id));
        }

        [Test]
        public void DeletePatient_Test_NotFoundException()
        {
            Assert.ThrowsAsync<System.Exception>(async () => await _patientRepository.Delete(999));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
} 