using OG.AIFileAnalyzer.Business.Historical;
using OG.AIFileAnalyzer.Persistence.DataAccess.Repositories.BaseRepository;
using OG.AIFileAnalyzer.Persistence.Services.Report;

namespace OG.AIFileAnalyzer.Tests.HistoricalBusinessTests
{
    public class AddHistorical : BaseTestClass
    {
        [Fact]
        public async Task AddHistorical_Method_Test()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var logRepositoryMock = new Mock<IBaseRepository<LogEntity>>();
            var historicalBusiness = new HistoricalBusiness(unitOfWorkMock.Object, new Mock<IReportService>().Object);
            var entity = new LogEntity();

            unitOfWorkMock.Setup(uow => uow.Repository<LogEntity>()).Returns(logRepositoryMock.Object);

            // Act
            await historicalBusiness.AddHistorical(entity);

            // Assert
            logRepositoryMock.Verify(repo => repo.AddEntity(entity), Times.Once);
            unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact]
        public async Task AddHistorical_Method_WhenUnitOfWorkIsNull_ReturnsException()
        {
            // Arrange
            IUnitOfWork unitOfWork = null;
            var reportServiceMock = new Mock<IReportService>();
            var historicalBusiness = new HistoricalBusiness(unitOfWork, reportServiceMock.Object);
            var entity = new LogEntity();

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () => await historicalBusiness.AddHistorical(entity));
        }

        [Fact]
        public async Task AddHistorical_Method_WhenLogEntityIsNull_ReturnsNullReferenceException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var logRepositoryMock = new Mock<IBaseRepository<LogEntity>>();
            var historicalBusiness = new HistoricalBusiness(unitOfWorkMock.Object, new Mock<IReportService>().Object);
            LogEntity entity = null;

            unitOfWorkMock.Setup(uow => uow.Repository<LogEntity>()).Returns(logRepositoryMock.Object);
            logRepositoryMock.Setup(repo => repo.AddEntity(It.IsAny<LogEntity>())).Throws<NullReferenceException>();

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () => await historicalBusiness.AddHistorical(entity));
        }

        [Fact]
        public async Task AddHistorical_Method_WhenAddEntityFails_ReturnsException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var logRepositoryMock = new Mock<IBaseRepository<LogEntity>>();
            var historicalBusiness = new HistoricalBusiness(unitOfWorkMock.Object, new Mock<IReportService>().Object);
            var entity = new LogEntity();

            unitOfWorkMock.Setup(uow => uow.Repository<LogEntity>()).Returns(logRepositoryMock.Object);
            logRepositoryMock.Setup(repo => repo.AddEntity(It.IsAny<LogEntity>())).Throws<Exception>();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await historicalBusiness.AddHistorical(entity));
        }

        [Fact]
        public async Task AddHistorical_Method_WhenUnitOfWorkCompleteFails_ReturnsException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var logRepositoryMock = new Mock<IBaseRepository<LogEntity>>();
            var historicalBusiness = new HistoricalBusiness(unitOfWorkMock.Object, new Mock<IReportService>().Object);
            var entity = new LogEntity();

            unitOfWorkMock.Setup(uow => uow.Repository<LogEntity>()).Returns(logRepositoryMock.Object);
            unitOfWorkMock.Setup(uow => uow.Complete()).Throws<Exception>();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await historicalBusiness.AddHistorical(entity));
        }

        [Fact]
        public async Task AddHistorical_Method_WhenUnitOfWorkCompleteIsCalledMultipleTimes()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var logRepositoryMock = new Mock<IBaseRepository<LogEntity>>();
            var historicalBusiness = new HistoricalBusiness(unitOfWorkMock.Object, new Mock<IReportService>().Object);
            var entity = new LogEntity();

            unitOfWorkMock.Setup(uow => uow.Repository<LogEntity>()).Returns(logRepositoryMock.Object);

            // Act
            await historicalBusiness.AddHistorical(entity);
            await historicalBusiness.AddHistorical(entity);

            // Assert
            unitOfWorkMock.Verify(uow => uow.Complete(), Times.Exactly(2));
        }

        [Fact]
        public async Task AddHistorical_UserAction_Test()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var logRepositoryMock = new Mock<IBaseRepository<LogEntity>>();
            var historicalBusiness = new HistoricalBusiness(unitOfWorkMock.Object, new Mock<IReportService>().Object);

            var entity = new LogEntity()
            {
                ActionType = ActionType.UserAction,
                Description = "Test",
                Details = "Test description",
                Date = DateTime.Now,
            };

            unitOfWorkMock.Setup(uow => uow.Repository<LogEntity>()).Returns(logRepositoryMock.Object);
            logRepositoryMock.Setup(repo => repo.AddEntity(It.IsAny<LogEntity>())).Returns(Task.CompletedTask);
            // Act
            await historicalBusiness.AddHistorical(entity);

            // Assert
            logRepositoryMock.Verify(repo => repo.AddEntity(entity), Times.Once);
            unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
        }

        [Theory]
        [InlineData(ActionType.UserAction)]
        [InlineData(ActionType.IA)]
        [InlineData(ActionType.DocumentUpload)]
        public async Task AddHistorical_Actions_Test(ActionType action)
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var logRepositoryMock = new Mock<IBaseRepository<LogEntity>>();
            var historicalBusiness = new HistoricalBusiness(unitOfWorkMock.Object, new Mock<IReportService>().Object);

            var entity = new LogEntity()
            {
                ActionType = action,
                Description = "Test",
                Details = "Test description",
                Date = DateTime.Now,
            };

            unitOfWorkMock.Setup(uow => uow.Repository<LogEntity>()).Returns(logRepositoryMock.Object);
            logRepositoryMock.Setup(repo => repo.AddEntity(It.IsAny<LogEntity>())).Returns(Task.CompletedTask);
            // Act
            await historicalBusiness.AddHistorical(entity);

            // Assert
            logRepositoryMock.Verify(repo => repo.AddEntity(entity), Times.Once);
            unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
        }
    }
}
