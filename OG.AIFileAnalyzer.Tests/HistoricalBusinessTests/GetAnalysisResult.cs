using OG.AIFileAnalyzer.Business.Historical;
using OG.AIFileAnalyzer.Persistence.DataAccess.Repositories.BaseRepository;
using OG.AIFileAnalyzer.Persistence.Services.Report;
using System.Linq.Expressions;

namespace OG.AIFileAnalyzer.Tests.HistoricalBusinessTests
{
    public class GetAnalysisResult : BaseTestClass
    {
        [Theory]
        [InlineData(DocType.Invoice)]
        [InlineData(DocType.GeneralText)]
        public async Task GetAnalysisResult_ReturnsAnalysisResponse(DocType doc)
        {
            // Arrange
            var hash = "sampleHash";
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var reportServiceMock = new Mock<IReportService>();
            var historicalBusMock = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            var expectedResult = new AnalysisResponseDTO
            {
                DocumentType = doc,
                Data = new Dictionary<string, string>
                {
                    { "key1", "value1" },
                    { "key2", "value2" }
                }
            };

            var fileEntity = new FileEntity
            {
                SHA256 = hash,
                DocumentType = doc,
                Anaysis = new List<FileAnaysisEntity>()
                {
                    new(){Key = "key1", Value = "value1"},
                    new(){Key = "key2", Value = "value2"}
                }
            };

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                             .ReturnsAsync(new List<FileEntity> { fileEntity });

            // Act

            var result = await historicalBusMock.GetAnalysisResult(hash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.DocumentType, result.DocumentType);
            Assert.Equal(expectedResult.Data, result.Data);
        }

        [Fact]
        public async Task GetAnalysisResult_ReturnsInvoiceAnalysisResponseWithSpecificKeys()
        {
            const string INVOICE_ID_KEY = "InvoiceId";
            const string INVOICE_TOTAL_KEY = "InvoiceTotal";
            // Arrange
            var hash = INVOICE_HASH;
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var reportServiceMock = new Mock<IReportService>();
            var historicalBusMock = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            var expectedResult = new AnalysisResponseDTO
            {
                DocumentType = DocType.Invoice,
                Data = new Dictionary<string, string>
                {
                    { INVOICE_ID_KEY, "value1" },
                    { INVOICE_TOTAL_KEY, "value2" }
                }
            };

            var fileEntity = new FileEntity
            {
                SHA256 = hash,
                DocumentType = DocType.Invoice,
                Anaysis = new List<FileAnaysisEntity>()
                {
                    new(){Key = INVOICE_ID_KEY, Value = "value1"},
                    new(){Key = INVOICE_TOTAL_KEY, Value = "value2"}
                }
            };

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>())).ReturnsAsync([fileEntity]);

            // Act

            var result = await historicalBusMock.GetAnalysisResult(hash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.DocumentType, result.DocumentType);
            Assert.Equal(expectedResult.Data, result.Data);
        }

        [Fact]
        public async Task GetAnalysisResult_ReturnsGeneralTextAnalysisResponseWithSpecificKeys()
        {
            const string SENTIMENT_KEY = "Sentiment";
            const string SUMMARY_KEY = "Summary";

            // Arrange
            var hash = INVOICE_HASH;
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var reportServiceMock = new Mock<IReportService>();
            var historicalBusMock = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            var expectedResult = new AnalysisResponseDTO
            {
                DocumentType = DocType.GeneralText,
                Data = new Dictionary<string, string>
                {
                    { SENTIMENT_KEY, "value1" },
                    { SUMMARY_KEY, "value2" }
                }
            };

            var fileEntity = new FileEntity
            {
                SHA256 = hash,
                DocumentType = DocType.GeneralText,
                Anaysis = new List<FileAnaysisEntity>()
                {
                    new(){Key = SENTIMENT_KEY, Value = "value1"},
                    new(){Key = SUMMARY_KEY, Value = "value2"}
                }
            };

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>())).ReturnsAsync([fileEntity]);

            // Act

            var result = await historicalBusMock.GetAnalysisResult(hash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.DocumentType, result.DocumentType);
            Assert.Equal(expectedResult.Data, result.Data);
        }

        [Fact]
        public async Task GetAnalysisResult_ReturnsEmptyInvoiceAnalysisResponse()
        {
            // Arrange
            var hash = INVOICE_HASH;
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var reportServiceMock = new Mock<IReportService>();
            var historicalBusMock = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>())).ReturnsAsync([]);

            // Act
            var result = await historicalBusMock.GetAnalysisResult(hash);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAnalysisResult_ReturnsEmptyGeneralTextAnalysisResponse()
        {
            // Arrange
            var hash = INVOICE_HASH;
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var reportServiceMock = new Mock<IReportService>();
            var historicalBusMock = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>())).ReturnsAsync([]);

            // Act
            var result = await historicalBusMock.GetAnalysisResult(hash);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAnalysisResult_ReturnsInvoiceEmptyAnalysisResponseWithSpecificKeys()
        {
            const string INVOICE_ID_KEY = "InvoiceId";
            const string INVOICE_TOTAL_KEY = "InvoiceTotal";
            // Arrange
            var hash = INVOICE_HASH;
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var reportServiceMock = new Mock<IReportService>();
            var historicalBusMock = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            var expectedResult = new AnalysisResponseDTO
            {
                DocumentType = DocType.Invoice,
                Data = new Dictionary<string, string>
                {
                    { INVOICE_ID_KEY, "value1" },
                    { INVOICE_TOTAL_KEY, "value2" }
                }
            };

            var fileEntity = new FileEntity
            {
                SHA256 = hash,
                DocumentType = DocType.Invoice,
                Anaysis = new List<FileAnaysisEntity>()
                {
                    new(){Key = new string(INVOICE_ID_KEY.Reverse().ToArray()), Value = "value1"},
                    new(){Key = new string(INVOICE_TOTAL_KEY.Reverse().ToArray()), Value = "value2"}
                }
            };

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>())).ReturnsAsync([fileEntity]);

            // Act

            var result = await historicalBusMock.GetAnalysisResult(hash);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task GetAnalysisResult_ReturnsGeneralTextEmptyAnalysisResponseWithSpecificKeys()
        {
            const string SENTIMENT_KEY = "Sentiment";
            const string SUMMARY_KEY = "Summary";

            // Arrange
            var hash = INVOICE_HASH;
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var reportServiceMock = new Mock<IReportService>();
            var historicalBusMock = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            var expectedResult = new AnalysisResponseDTO
            {
                DocumentType = DocType.GeneralText,
                Data = new Dictionary<string, string>
                {
                    { SENTIMENT_KEY, "value1" },
                    { SUMMARY_KEY, "value2" }
                }
            };

            var fileEntity = new FileEntity
            {
                SHA256 = hash,
                DocumentType = DocType.GeneralText,
                Anaysis = new List<FileAnaysisEntity>()
                {
                    new(){Key = new string(SENTIMENT_KEY.Reverse().ToArray()), Value = "value1"},
                    new(){Key = new string(SUMMARY_KEY.Reverse().ToArray()), Value = "value2"}
                }
            };

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>())).ReturnsAsync([fileEntity]);

            // Act

            var result = await historicalBusMock.GetAnalysisResult(hash);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
        }

        [Theory]
        [InlineData(DocType.Invoice)]
        [InlineData(DocType.GeneralText)]
        public async Task GetAnalysisResult_ReturnsUnexpectedData(DocType doc)
        {
            // Arrange
            var hash = "sampleHash";
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var reportServiceMock = new Mock<IReportService>();
            var historicalBusMock = new HistoricalBusiness(unitOfWorkMock.Object, reportServiceMock.Object);

            var expectedResult = new AnalysisResponseDTO
            {
                DocumentType = doc,
                Data = new Dictionary<string, string>()
            };

            var fileEntity = new FileEntity
            {
                SHA256 = hash,
                DocumentType = doc,
                Anaysis = new List<FileAnaysisEntity>()
                {
                    new(){Key = "key1", Value = "value1"},
                    new(){Key = "key2", Value = "value2"}
                }
            };

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                             .ReturnsAsync(new List<FileEntity> { fileEntity });

            // Act

            var result = await historicalBusMock.GetAnalysisResult(hash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.DocumentType, result.DocumentType);
            Assert.NotEqual(expectedResult.Data, result.Data);
        }
    }
}
