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
    public class DoctorRepoTest
    {
        private ClinicContext _context;
        private DoctorRepository _doctorRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ClinicContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Doctor")
                .Options;
            _context = new ClinicContext(options);
            _doctorRepository = new DoctorRepository(_context);
        }

        [Test]
        public async Task AddDoctor_Test()
        {
            var doctor = new Doctor { Name = "Test", Email = "test@email.com", YearsOfExperience = 5 };
            var result = await _doctorRepository.Add(doctor);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.GreaterThan(0));
        }

        [Test]
        public async Task GetDoctor_Test()
        {
            var doctor = new Doctor { Name = "Test", Email = "test@email.com", YearsOfExperience = 5 };
            await _doctorRepository.Add(doctor);
            var result = await _doctorRepository.Get(doctor.Id);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Test"));
        }

        [Test]
        public void GetDoctor_Test_NotFoundException()
        {
            Assert.ThrowsAsync<System.Exception>(async () => await _doctorRepository.Get(999));
        }

        [Test]
        public async Task GetAllDoctors_Test()
        {
            await _doctorRepository.Add(new Doctor { Name = "Test1", Email = "a@email.com", YearsOfExperience = 1 });
            await _doctorRepository.Add(new Doctor { Name = "Test2", Email = "b@email.com", YearsOfExperience = 2 });
            var result = await _doctorRepository.GetAll();
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GetAllDoctors_Test_EmptyException()
        {
            Assert.ThrowsAsync<System.Exception>(async () => await _doctorRepository.GetAll());
        }

        [Test]
        public async Task UpdateDoctor_Test()
        {
            var doctor = new Doctor { Name = "Test", Email = "test@email.com", YearsOfExperience = 5 };
            await _doctorRepository.Add(doctor);
            doctor.Name = "Updated";
            var result = await _doctorRepository.Update(doctor.Id, doctor);
            Assert.That(result.Name, Is.EqualTo("Updated"));
        }

        [Test]
        public void UpdateDoctor_Test_NotFoundException()
        {
            var doctor = new Doctor { Id = 999, Name = "NotFound" };
            Assert.ThrowsAsync<System.Exception>(async () => await _doctorRepository.Update(doctor.Id, doctor));
        }

        [Test]
        public async Task DeleteDoctor_Test()
        {
            var doctor = new Doctor { Name = "Test", Email = "test@email.com", YearsOfExperience = 5 };
            await _doctorRepository.Add(doctor);
            var result = await _doctorRepository.Delete(doctor.Id);
            Assert.That(result, Is.Not.Null);
            Assert.ThrowsAsync<System.Exception>(async () => await _doctorRepository.Get(doctor.Id));
        }

        [Test]
        public void DeleteDoctor_Test_NotFoundException()
        {
            Assert.ThrowsAsync<System.Exception>(async () => await _doctorRepository.Delete(999));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}