using FirstAPI.Contexts;
using FirstAPI.Models;
using FirstAPI.Repositories;
using FirstAPI.Interfaces;
using FirstAPI.Services;
using FirstAPI.Models.DTOs;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using FirstAPI.Misc;
using Microsoft.EntityFrameworkCore;
using Moq;
using AutoMapper;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FirstAPI.Test;

public class DoctorServiceTest
{
    private Mock<IRepository<int, Doctor>> _doctorRepositoryMock;
    private Mock<IRepository<int, Speciality>> _specialityRepositoryMock;
    private Mock<IRepository<int, DoctorSpeciality>> _doctorSpecialityRepositoryMock;
    private Mock<IRepository<string, User>> _userRepositoryMock;
    private Mock<IOtherContextFunctionalities> _otherContextFunctionalitiesMock;
    private Mock<IEncryptionService> _encryptionServiceMock;
    private Mock<IMapper> _mapperMock;
    private DoctorService _doctorService;

    [SetUp]
    public void Setup()
    {
        _doctorRepositoryMock = new Mock<IRepository<int, Doctor>>();
        _specialityRepositoryMock = new Mock<IRepository<int, Speciality>>();
        _doctorSpecialityRepositoryMock = new Mock<IRepository<int, DoctorSpeciality>>();
        _userRepositoryMock = new Mock<IRepository<string, User>>();
        _otherContextFunctionalitiesMock = new Mock<IOtherContextFunctionalities>();
        _encryptionServiceMock = new Mock<IEncryptionService>();
        _mapperMock = new Mock<IMapper>();
        _doctorService = new DoctorService(
            _doctorRepositoryMock.Object,
            _specialityRepositoryMock.Object,
            _doctorSpecialityRepositoryMock.Object,
            _userRepositoryMock.Object,
            _otherContextFunctionalitiesMock.Object,
            _encryptionServiceMock.Object,
            _mapperMock.Object
        );
    }

    [Test]
    public async Task AddDoctor_Test()
    {
        var dto = new DoctorAddRequestDTO
        {
            Name = "doctor",
            Email = "doc@email.com",
            Password = "pass",
            YearsOfExperience = 5,
            Specialities = new List<SpecialityAddRequestDTO> { new SpecialityAddRequestDTO { Name = "General" } }
        };
        var user = new User { Username = dto.Email, Password = new byte[] { 1, 2 }, HashKey = new byte[] { 3, 4 }, Role = "Doctor" };
        var doctor = new Doctor { Name = dto.Name, Email = dto.Email, YearsOfExperience = dto.YearsOfExperience };
        var speciality = new Speciality { Id = 1, Name = "General" };
        var doctorSpeciality = new DoctorSpeciality { SerialNumber = 1, DoctorId = 1, SpecialityId = 1 };
        
        _mapperMock.Setup(m => m.Map<DoctorAddRequestDTO, User>(dto)).Returns(user);

        _encryptionServiceMock.Setup(e => e.EncryptData(It.IsAny<EncryptModel>())).ReturnsAsync(new EncryptModel { EncryptedData = new byte[] { 1, 2 }, HashKey = new byte[] { 3, 4 } });

        _userRepositoryMock.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync(user);
        _doctorRepositoryMock.Setup(r => r.Add(It.IsAny<Doctor>())).ReturnsAsync(doctor);

        _specialityRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<Speciality>());

        _specialityRepositoryMock.Setup(r => r.Add(It.IsAny<Speciality>())).ReturnsAsync(speciality);
        _doctorSpecialityRepositoryMock.Setup(r => r.Add(It.IsAny<DoctorSpeciality>())).ReturnsAsync(doctorSpeciality);
        var result = await _doctorService.AddDoctor(dto);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(dto.Name));
    }

    [Test]
    public void GetDoctorByName_Test_NullOrEmptyException()
    {
        Assert.ThrowsAsync<ArgumentException>(() => _doctorService.GetDoctorByName(null));
        Assert.ThrowsAsync<ArgumentException>(() => _doctorService.GetDoctorByName(" "));
    }

    [Test]
    public async Task GetDoctorByName_Test()
    {
        var name = "doctor";
        var doctors = new List<Doctor> { new Doctor { Name = name } };
        _doctorRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(doctors);

        var result = await _doctorService.GetDoctorByName(name);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(name));
    }

    [Test]
    public async Task GetDoctorsBySpeciality_Test()
    {
        var speciality = "General";
        var dtos = new List<DoctorsBySpecialityResponseDTO> { new DoctorsBySpecialityResponseDTO { Dname = "doctor", Yoe = 5, Id = 1 } };
        _otherContextFunctionalitiesMock.Setup(o => o.GetDoctorsBySpeciality(speciality)).ReturnsAsync(dtos);

        var result = await _doctorService.GetDoctorsBySpeciality(speciality);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result.First().Dname, Is.EqualTo("doctor"));
    }

    [Test]
    public void GetDoctorByEmail_Test_NullOrEmptyException()
    {
        Assert.ThrowsAsync<ArgumentException>(() => _doctorService.GetDoctorByEmail(null));
        Assert.ThrowsAsync<ArgumentException>(() => _doctorService.GetDoctorByEmail(" "));
    }

    [Test]
    public async Task GetDoctorByEmail_Test()
    {
        var email = "doc@email.com";
        var doctors = new List<Doctor> { new Doctor { Email = email } };
        _doctorRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(doctors);
        var result = await _doctorService.GetDoctorByEmail(email);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Email, Is.EqualTo(email));
    }

}