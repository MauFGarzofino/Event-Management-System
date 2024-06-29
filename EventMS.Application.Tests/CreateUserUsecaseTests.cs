using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EventMS.Application.DTOs.UsersDto;
using EventMS.Application.UseCases.UserUseCases;
using EventMS.Domain.Entities;
using EventMS.Domain.Interfaces;
using Moq;
using Xunit;

public class CreateUserUseCaseTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly CreateUserUseCase _createUserUseCase;

    public CreateUserUseCaseTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ClaimsPrincipal, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.FindFirst(ClaimTypes.NameIdentifier).Value)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FindFirst(ClaimTypes.GivenName).Value))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.FindFirst(ClaimTypes.Surname).Value))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.FindFirst(ClaimTypes.Email).Value))
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.FindFirst("preferred_username").Value))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.FindFirst(ClaimTypes.Role).Value));
        });

        _mapper = mapperConfig.CreateMapper();
        _userRepositoryMock = new Mock<IUserRepository>();
        _createUserUseCase = new CreateUserUseCase(_mapper, _userRepositoryMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldCreateUser_WhenClaimsAreValid()
    {
        // Arrange
        var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.GivenName, "John"),
            new Claim(ClaimTypes.Surname, "Doe"),
            new Claim(ClaimTypes.Email, "john.doe@example.com"),
            new Claim("preferred_username", "johndoe"),
            new Claim(ClaimTypes.Role, "User")
        }));

        // Act
        var user = _createUserUseCase.Execute(userClaims);

        // Assert
        _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<User>()), Times.Once);
        Assert.NotNull(user);
        Assert.Equal("John", user.Name);
        Assert.Equal("Doe", user.Surname);
        Assert.Equal("john.doe@example.com", user.Email);
        Assert.Equal("johndoe", user.Nickname);
        Assert.Equal("User", user.Role);
    }
}
