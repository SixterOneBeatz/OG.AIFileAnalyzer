using OG.AIFileAnalyzer.Business.Historical;
using OG.AIFileAnalyzer.Persistence.DataAccess.Repositories.BaseRepository;
using OG.AIFileAnalyzer.Persistence.Services.Report;
using System.Linq.Expressions;

namespace OG.AIFileAnalyzer.Tests.HistoricalBusinessTests
{
    public class GetHistorical : BaseTestClass
    {
        [Fact]
        public async Task GetHistorical_Method_ReturnsAllHistoricalResult()
        {
            // Arrange
            var filter = new HistoricalFilterDTO
            {
                Action = ActionType.All,
                DateStart = new DateTime(2023, 1, 1),
                DateEnd = new DateTime(2023, 12, 31),
                Description = "sample description",
                Skip = 0,
                Take = 10
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var reportServiceMock = new Mock<IReportService>();
            var logRepositoryMock = new Mock<IBaseRepository<LogEntity>>();
            var logEntities = new List<LogEntity>
            {
                new LogEntity { ActionType = ActionType.IA, Date = new DateTime(2023, 6, 15), Description = "sample description 1" },
                new LogEntity { ActionType = ActionType.DocumentUpload, Date = new DateTime(2023, 8, 20), Description = "sample description 2" }
            };

            unitOfWorkMock.Setup(uow => uow.Repository<LogEntity>()).Returns(logRepositoryMock.Object);
            logRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<LogEntity, bool>>>())).ReturnsAsync((logEntities, logEntities.Count));

            var businessLogic = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            // Act
            var result = await businessLogic.GetHistorical(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(logEntities.Count, result.TotalRows);
            Assert.Equal(logEntities, result.Rows);
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
                Description = "sample description",
                Skip = 0,
                Take = 10
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

            var businessLogic = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            // Act
            var result = await businessLogic.GetHistorical(filter);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Rows);
            Assert.NotEqual(logEntities.Count, result.TotalRows);
            Assert.True(result.Rows.TrueForAll(x => x.ActionType == action));
        }

        [Theory]
        [InlineData(ActionType.UserAction)]
        [InlineData(ActionType.IA)]
        [InlineData(ActionType.DocumentUpload)]
        public async Task GetHistorical_ReturnsHistoricalResultByAction_Take1(ActionType action)
        {
            // Arrange
            var filter = new HistoricalFilterDTO
            {
                Action = action,
                DateStart = new DateTime(2023, 1, 1),
                DateEnd = new DateTime(2023, 12, 31),
                Description = "sample description",
                Skip = 0,
                Take = 1
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var reportServiceMock = new Mock<IReportService>();
            var logRepositoryMock = new Mock<IBaseRepository<LogEntity>>();
            var logEntities = new List<LogEntity>
            {
                new LogEntity { ActionType = ActionType.IA, Date = new DateTime(2023, 6, 15), Description = "sample description 1" },
                new LogEntity { ActionType = ActionType.DocumentUpload, Date = new DateTime(2023, 8, 20), Description = "sample description 2" },
                new LogEntity { ActionType = ActionType.UserAction, Date = new DateTime(2023, 12, 20), Description = "sample description 3" },
                new LogEntity { ActionType = ActionType.IA, Date = new DateTime(2023, 6, 15), Description = "sample description 3" },
                new LogEntity { ActionType = ActionType.DocumentUpload, Date = new DateTime(2023, 8, 20), Description = "sample description 5" },
                new LogEntity { ActionType = ActionType.UserAction, Date = new DateTime(2023, 12, 20), Description = "sample description 6" }
            };

            var onlyByAction = logEntities.FindAll(x => x.ActionType == action).Take(filter.Take).ToList();

            unitOfWorkMock.Setup(uow => uow.Repository<LogEntity>()).Returns(logRepositoryMock.Object);
            logRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<LogEntity, bool>>>()))
                .ReturnsAsync((onlyByAction, onlyByAction.Count));

            var businessLogic = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            // Act
            var result = await businessLogic.GetHistorical(filter);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Rows);
            Assert.Equal(result.TotalRows, filter.Take);
            Assert.True(result.Rows.TrueForAll(x => x.ActionType == action));
        }

        [Theory]
        [InlineData(ActionType.UserAction)]
        [InlineData(ActionType.IA)]
        [InlineData(ActionType.DocumentUpload)]
        public async Task GetHistorical_ReturnsHistoricalResultSkipTake(ActionType action)
        {
            // Arrange
            var filter = new HistoricalFilterDTO
            {
                Action = action,
                DateStart = new DateTime(2023, 1, 1),
                DateEnd = new DateTime(2023, 12, 31),
                Description = "sample description",
                Skip = 10,
                Take = 1
            };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var reportServiceMock = new Mock<IReportService>();
            var logRepositoryMock = new Mock<IBaseRepository<LogEntity>>();
            var logEntities = new List<LogEntity>();

            for (int i = 0; i < 11; i++)
            {
                logEntities.Add(new LogEntity { ActionType = action, Date = new DateTime(2023, 6, 15), Description = $"sample description {i}" });
            }

            var pagedResult = logEntities.FindAll(x => x.ActionType == action).Skip(filter.Skip).Take(filter.Take).ToList();

            unitOfWorkMock.Setup(uow => uow.Repository<LogEntity>()).Returns(logRepositoryMock.Object);
            logRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<LogEntity, bool>>>()))
                .ReturnsAsync((pagedResult, pagedResult.Count));

            var businessLogic = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            // Act
            var result = await businessLogic.GetHistorical(filter);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Rows);
            Assert.Equal(result.TotalRows, filter.Take);
        }
    }
}
