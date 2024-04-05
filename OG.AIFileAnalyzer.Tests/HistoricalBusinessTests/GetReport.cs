using OG.AIFileAnalyzer.Business.Historical;
using OG.AIFileAnalyzer.Persistence.DataAccess.Repositories.BaseRepository;
using OG.AIFileAnalyzer.Persistence.Services.Report;
using System.Linq.Expressions;

namespace OG.AIFileAnalyzer.Tests.HistoricalBusinessTests
{
    public class GetReport : BaseTestClass
    {
        [Fact]
        public async Task GetHistorical_ReturnsAllHistoricalResults()
        {
            // Arrange
            var filter = new HistoricalFilterDTO
            {
                Action = ActionType.All,
                DateStart = new DateTime(2023, 1, 1),
                DateEnd = new DateTime(2023, 12, 31),
                Description = "sample description"
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var reportServiceMock = new Mock<IReportService>();
            var logRepositoryMock = new Mock<IBaseRepository<LogEntity>>();
            var logEntities = new List<LogEntity>
            {
                new LogEntity { ActionType = ActionType.IA, Date = new DateTime(2023, 6, 15), Description = "sample description 1" },
                new LogEntity { ActionType = ActionType.DocumentUpload, Date = new DateTime(2023, 8, 20), Description = "sample description 2" },
                new LogEntity { ActionType = ActionType.UserAction, Date = new DateTime(2023, 12, 20), Description = "sample description 3" }
            };

            unitOfWorkMock.Setup(uow => uow.Repository<LogEntity>()).Returns(logRepositoryMock.Object);
            logRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<LogEntity, bool>>>()))
                .ReturnsAsync((logEntities, logEntities.Count));

            reportServiceMock.Setup(rs => rs.ExportToExcel(It.IsAny<List<LogEntity>>())).Returns(new MemoryStream());

            var businessLogic = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            // Act
            var result = await businessLogic.GetReport(filter);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(ActionType.UserAction)]
        [InlineData(ActionType.IA)]
        [InlineData(ActionType.DocumentUpload)]
        public async Task GetHistorical_ReturnsHistoricalResultByAction(ActionType action)
        {
            // Arrange
            var filter = new HistoricalFilterDTO
            {
                Action = action,
                DateStart = new DateTime(2023, 1, 1),
                DateEnd = new DateTime(2023, 12, 31),
                Description = "sample description"
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var reportServiceMock = new Mock<IReportService>();
            var logRepositoryMock = new Mock<IBaseRepository<LogEntity>>();
            var logEntities = new List<LogEntity>
            {
                new LogEntity { ActionType = ActionType.IA, Date = new DateTime(2023, 6, 15), Description = "sample description 1" },
                new LogEntity { ActionType = ActionType.DocumentUpload, Date = new DateTime(2023, 8, 20), Description = "sample description 2" },
                new LogEntity { ActionType = ActionType.UserAction, Date = new DateTime(2023, 12, 20), Description = "sample description 3" }
            };

            var onlyByAction = logEntities.FindAll(x => x.ActionType == action);

            unitOfWorkMock.Setup(uow => uow.Repository<LogEntity>()).Returns(logRepositoryMock.Object);
            logRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<LogEntity, bool>>>()))
                .ReturnsAsync((onlyByAction, onlyByAction.Count));

            reportServiceMock.Setup(rs => rs.ExportToExcel(It.IsAny<List<LogEntity>>())).Returns(new MemoryStream());

            var businessLogic = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            // Act
            var result = await businessLogic.GetReport(filter);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(ActionType.UserAction)]
        [InlineData(ActionType.IA)]
        [InlineData(ActionType.DocumentUpload)]
        public async Task GetHistorical_ReturnsHistoricalResultByAction_ThrowsException(ActionType action)
        {
            // Arrange
            var filter = new HistoricalFilterDTO
            {
                Action = action,
                DateStart = new DateTime(2023, 1, 1),
                DateEnd = new DateTime(2023, 12, 31),
                Description = "sample description"
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var reportServiceMock = new Mock<IReportService>();
            var logRepositoryMock = new Mock<IBaseRepository<LogEntity>>();
            var logEntities = new List<LogEntity>
            {
                new LogEntity { ActionType = ActionType.IA, Date = new DateTime(2023, 6, 15), Description = "sample description 1" },
                new LogEntity { ActionType = ActionType.DocumentUpload, Date = new DateTime(2023, 8, 20), Description = "sample description 2" },
                new LogEntity { ActionType = ActionType.UserAction, Date = new DateTime(2023, 12, 20), Description = "sample description 3" }
            };

            var onlyByAction = logEntities.FindAll(x => x.ActionType == action);

            unitOfWorkMock.Setup(uow => uow.Repository<LogEntity>()).Returns(logRepositoryMock.Object);
            logRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<LogEntity, bool>>>()))
                .ReturnsAsync((onlyByAction, onlyByAction.Count));

            reportServiceMock.Setup(rs => rs.ExportToExcel(It.IsAny<List<LogEntity>>())).Throws<Exception>();

            var businessLogic = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await businessLogic.GetReport(filter));
        }

        [Theory]
        [InlineData(ActionType.UserAction)]
        [InlineData(ActionType.IA)]
        [InlineData(ActionType.DocumentUpload)]
        public async Task GetHistorical_ReturnsEmptyHistoricalResultByAction(ActionType action)
        {
            // Arrange
            var filter = new HistoricalFilterDTO
            {
                Action = action,
                DateStart = new DateTime(2023, 1, 1),
                DateEnd = new DateTime(2023, 12, 31),
                Description = "sample description"
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var reportServiceMock = new Mock<IReportService>();
            var logRepositoryMock = new Mock<IBaseRepository<LogEntity>>();
            var logEntities = new List<LogEntity>();

            var onlyByAction = logEntities.FindAll(x => x.ActionType == action);

            unitOfWorkMock.Setup(uow => uow.Repository<LogEntity>()).Returns(logRepositoryMock.Object);
            logRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<LogEntity, bool>>>()))
                .ReturnsAsync((onlyByAction, onlyByAction.Count));

            reportServiceMock.Setup(rs => rs.ExportToExcel(It.IsAny<List<LogEntity>>())).Returns(new MemoryStream());

            var businessLogic = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            // Act
            var result = await businessLogic.GetReport(filter);

            // Assert
            Assert.NotNull(result);
        }
    }
}
