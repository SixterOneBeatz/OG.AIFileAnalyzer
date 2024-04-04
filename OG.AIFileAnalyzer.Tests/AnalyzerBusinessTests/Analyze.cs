using OG.AIFileAnalyzer.Persistence.DataAccess.Repositories.BaseRepository;
using OG.AIFileAnalyzer.Persistence.Services.AzureAI;
using System.Linq.Expressions;


namespace OG.AIFileAnalyzer.Tests.AnalyzerBusinessTests
{
    public class Analyze : BaseTestClass
    {
        [Fact]
        public async Task Analyze_WithExistingInvoiceAnalysisInDatabase_ReturnsExistingAnalysis()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockFileRepository = new Mock<IBaseRepository<FileEntity>>();
            var mockLogRepository = new Mock<IBaseRepository<LogEntity>>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);
            var expectedDictionary = new Dictionary<string, string>
            {
                { "InvoiceId", "Value" },
                { "InvoiceTotal", "Value" }
            };
            var existingHash = INVOICE_HASH;
            var existingAnalysis = new AnalysisResponseDTO
            {
                DocumentType = DocType.Invoice,
                Data = expectedDictionary
            };
            var existingEntity = new List<FileEntity>
            {
                new FileEntity { SHA256 = existingHash,  DocumentType = DocType.Invoice, Anaysis = new List<FileAnaysisEntity>() }
            };

            mockFileRepository.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<FileEntity, bool>>>(),
                It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                .ReturnsAsync(existingEntity);

            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>()).Returns(mockFileRepository.Object);
            mockUnitOfWork.Setup(uow => uow.Repository<LogEntity>()).Returns(mockLogRepository.Object);
            mockAzureAIService.Setup(az => az.RunInvoiceAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(expectedDictionary);

            // Act
            var result = await analyzerBusiness.Analyze(INVOICE_BASE64);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingAnalysis.DocumentType, result.DocumentType);
        }

        [Fact]
        public async Task Analyze_WithNewInvoiceAnalysis_ReturnsInvoiceAnalysis()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockFileRepository = new Mock<IBaseRepository<FileEntity>>();
            var mockLogRepository = new Mock<IBaseRepository<LogEntity>>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);
            var expectedDictionary = new Dictionary<string, string>
            {
                    { "InvoiceId", "Value" },
                    { "InvoiceTotal", "Value" }
            };
            var expectedResult = new AnalysisResponseDTO
            {
                DocumentType = DocType.Invoice,
                Data = expectedDictionary
            };

            mockFileRepository.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<FileEntity, bool>>>(),
                It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                .ReturnsAsync([]);

            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>()).Returns(mockFileRepository.Object);
            mockUnitOfWork.Setup(uow => uow.Repository<LogEntity>()).Returns(mockLogRepository.Object);
            mockAzureAIService.Setup(az => az.RunInvoiceAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(expectedDictionary);

            // Act
            var result = await analyzerBusiness.Analyze(INVOICE_BASE64);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.DocumentType, result.DocumentType);
        }

        [Fact]
        public async Task Analyze_WithExistingGeneralTextAnalysisInDatabase_ReturnsExistingAnalysis()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockFileRepository = new Mock<IBaseRepository<FileEntity>>();
            var mockLogRepository = new Mock<IBaseRepository<LogEntity>>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);
            var expectedDictionary = new Dictionary<string, string>
            {
                { "Sentiment", "Value" },
                { "Summary 1:", "Value" }
            };
            var existingHash = TEXT_HASH;
            var existingAnalysis = new AnalysisResponseDTO
            {
                DocumentType = DocType.GeneralText,
                Data = expectedDictionary
            };
            var existingEntity = new List<FileEntity>
            {
                new FileEntity { SHA256 = existingHash,  DocumentType = DocType.GeneralText , Anaysis = new List<FileAnaysisEntity>() }
            };

            mockFileRepository.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<FileEntity, bool>>>(),
                It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                .ReturnsAsync(existingEntity);

            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>()).Returns(mockFileRepository.Object);
            mockUnitOfWork.Setup(uow => uow.Repository<LogEntity>()).Returns(mockLogRepository.Object);
            mockAzureAIService.Setup(az => az.RunInvoiceAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync([]);
            mockAzureAIService.Setup(az => az.RunTextAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(expectedDictionary);

            // Act
            var result = await analyzerBusiness.Analyze(TEXT_BASE64);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingAnalysis.DocumentType, result.DocumentType);
        }

        [Fact]
        public async Task Analyze_WithNewGeneralTextAnalysisInDatabase_ReturnsGeneralTextAnalysis()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockFileRepository = new Mock<IBaseRepository<FileEntity>>();
            var mockLogRepository = new Mock<IBaseRepository<LogEntity>>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);
            var expectedDictionary = new Dictionary<string, string>
            {
                { "Sentiment", "Value" },
                { "Summary 1:", "Value" }
            };

            var expectedAnalysis = new AnalysisResponseDTO
            {
                DocumentType = DocType.GeneralText,
                Data = expectedDictionary
            };

            mockFileRepository.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<FileEntity, bool>>>(),
                It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                .ReturnsAsync([]);

            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>()).Returns(mockFileRepository.Object);
            mockUnitOfWork.Setup(uow => uow.Repository<LogEntity>()).Returns(mockLogRepository.Object);
            mockAzureAIService.Setup(az => az.RunInvoiceAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync([]);
            mockAzureAIService.Setup(az => az.RunTextAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(expectedDictionary);

            // Act
            var result = await analyzerBusiness.Analyze(TEXT_BASE64);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAnalysis.DocumentType, result.DocumentType);
        }

        [Fact]
        public async Task Analyze_WithMultipleExistingInvoiceAnalysesInDatabase_ReturnsCorrectExistingAnalysis()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockFileRepository = new Mock<IBaseRepository<FileEntity>>();
            var mockLogRepository = new Mock<IBaseRepository<LogEntity>>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);

            var expectedDictionary = new Dictionary<string, string>
            {
                    { "InvoiceId", "Value1" },
                    { "InvoiceTotal", "Value1" }
            };

            var existingAnalysis1 = new AnalysisResponseDTO
            {
                DocumentType = DocType.Invoice,
                Data = expectedDictionary
            };
            var existingAnalysis2 = new AnalysisResponseDTO
            {
                DocumentType = DocType.Invoice,
                Data = new()
                {
                    { "InvoiceId", "Value2" },
                    { "InvoiceTotal", "Value2" }
                }
            };
            var existingEntity = new List<FileEntity>
            {
                new FileEntity 
                { 
                    SHA256 = INVOICE_HASH,  
                    DocumentType = DocType.Invoice, 
                    Anaysis = expectedDictionary.Select(x => new FileAnaysisEntity
                    {
                        Key = x.Key,    
                        Value = x.Value
                    }).ToList()
                },
                new FileEntity { SHA256 = new string(INVOICE_HASH.Reverse().ToArray()),  DocumentType = DocType.GeneralText, Anaysis = new List<FileAnaysisEntity>() }
            };

            mockFileRepository.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<FileEntity, bool>>>(),
                It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                .ReturnsAsync(existingEntity);

            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>()).Returns(mockFileRepository.Object);
            mockUnitOfWork.Setup(uow => uow.Repository<LogEntity>()).Returns(mockLogRepository.Object);
            mockAzureAIService.Setup(az => az.RunInvoiceAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(expectedDictionary);

            // Act
            var result = await analyzerBusiness.Analyze(INVOICE_BASE64);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingAnalysis1.DocumentType, result.DocumentType);
            Assert.Equal(existingAnalysis1.Data, result.Data); // Assuming Data comparison is implemented
        }

        [Fact]
        public async Task Analyze_WithMultipleExistingGeneralTextAnalysesInDatabase_ReturnsCorrectExistingAnalysis()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockFileRepository = new Mock<IBaseRepository<FileEntity>>();
            var mockLogRepository = new Mock<IBaseRepository<LogEntity>>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);

            var expectedDictionary = new Dictionary<string, string>
            {
                    { "Sentiment", "Value1" },
                    { "Summary 1:", "Value1" }
            };

            var existingAnalysis1 = new AnalysisResponseDTO
            {
                DocumentType = DocType.GeneralText,
                Data = expectedDictionary
            };
            var existingAnalysis2 = new AnalysisResponseDTO
            {
                DocumentType = DocType.GeneralText,
                Data = new()
                {
                    { "Sentiment", "Value2" },
                    { "Summary 1:", "Value2" }
                }
            };
            var existingEntity = new List<FileEntity>
            {
                new FileEntity
                {
                    SHA256 = TEXT_HASH,
                    DocumentType = DocType.GeneralText,
                    Anaysis = expectedDictionary.Select(x => new FileAnaysisEntity
                    {
                        Key = x.Key,
                        Value = x.Value
                    }).ToList()
                },
                new FileEntity { SHA256 = new string(TEXT_HASH.Reverse().ToArray()),  DocumentType = DocType.GeneralText, Anaysis = new List<FileAnaysisEntity>() }
            };

            mockFileRepository.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<FileEntity, bool>>>(),
                It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                .ReturnsAsync(existingEntity);

            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>()).Returns(mockFileRepository.Object);
            mockUnitOfWork.Setup(uow => uow.Repository<LogEntity>()).Returns(mockLogRepository.Object);
            mockAzureAIService.Setup(az => az.RunInvoiceAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync([]);
            mockAzureAIService.Setup(az => az.RunTextAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(expectedDictionary);

            // Act
            var result = await analyzerBusiness.Analyze(INVOICE_BASE64);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingAnalysis1.DocumentType, result.DocumentType);
            Assert.Equal(existingAnalysis1.Data, result.Data); // Assuming Data comparison is implemented
        }

        [Fact]
        public async Task Analyze_WithNewInvoiceAnalysis_ReturnsInvoiceAnalysis_WithDifferentInvoiceData()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockFileRepository = new Mock<IBaseRepository<FileEntity>>();
            var mockLogRepository = new Mock<IBaseRepository<LogEntity>>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);
            var expectedDictionary = new Dictionary<string, string>
            {
                { "InvoiceId", "Value" },
                { "InvoiceTotal", "Value" }
            };
            var existingHash = INVOICE_HASH;
            var existingAnalysis = new AnalysisResponseDTO
            {
                DocumentType = DocType.Invoice,
                Data = expectedDictionary
            };
            var existingEntity = new List<FileEntity>
            {
                new FileEntity { SHA256 = existingHash,  DocumentType = DocType.Invoice, Anaysis = new List<FileAnaysisEntity>() }
            };

            mockFileRepository.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<FileEntity, bool>>>(),
                It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                .ReturnsAsync(existingEntity);

            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>()).Returns(mockFileRepository.Object);
            mockUnitOfWork.Setup(uow => uow.Repository<LogEntity>()).Returns(mockLogRepository.Object);
            mockAzureAIService.Setup(az => az.RunInvoiceAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(new Dictionary<string, string>
            {
                { "InvoiceId", "Value1" },
                { "InvoiceTotal", "Value1" }
            });

            // Act
            var result = await analyzerBusiness.Analyze(INVOICE_BASE64);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(existingAnalysis.Data, result.Data);
        }

        [Fact]
        public async Task Analyze_WithNewGeneralTextAnalysis_ReturnsGeneralTextAnalysis_WithDifferentInvoiceData()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockFileRepository = new Mock<IBaseRepository<FileEntity>>();
            var mockLogRepository = new Mock<IBaseRepository<LogEntity>>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);
            var expectedDictionary = new Dictionary<string, string>
            {
                { "Sentiment", "Value" },
                { "Summary 1:", "Value" }
            };

            var expectedAnalysis = new AnalysisResponseDTO
            {
                DocumentType = DocType.GeneralText,
                Data = expectedDictionary
            };

            mockFileRepository.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<FileEntity, bool>>>(),
                It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                .ReturnsAsync([]);

            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>()).Returns(mockFileRepository.Object);
            mockUnitOfWork.Setup(uow => uow.Repository<LogEntity>()).Returns(mockLogRepository.Object);
            mockAzureAIService.Setup(az => az.RunInvoiceAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync([]);
            mockAzureAIService.Setup(az => az.RunTextAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(new Dictionary<string, string>
            {
                { "Sentiment", "Value1" },
                { "Summary 1:", "Value1" }
            });

            // Act
            var result = await analyzerBusiness.Analyze(TEXT_BASE64);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(expectedAnalysis.Data, result.Data);
        }

        [Fact]
        public async Task Analyze_WithNoExistingInvoiceAnalysisInDatabase_ReturnsExpected()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockFileRepository = new Mock<IBaseRepository<FileEntity>>();
            var mockLogRepository = new Mock<IBaseRepository<LogEntity>>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);
            var expectedDictionary = new Dictionary<string, string>
            {
                { "InvoiceId", "Value" },
                { "InvoiceTotal", "Value" }
            };

            mockFileRepository.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<FileEntity, bool>>>(),
                It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                .ReturnsAsync([]);

            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>()).Returns(mockFileRepository.Object);
            mockUnitOfWork.Setup(uow => uow.Repository<LogEntity>()).Returns(mockLogRepository.Object);
            mockAzureAIService.Setup(az => az.RunInvoiceAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(expectedDictionary);

            // Act
            var result = await analyzerBusiness.Analyze(INVOICE_BASE64);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDictionary, result.Data);
        }

        [Fact]
        public async Task Analyze_WithNoExistingGeneralTextAnalysisInDatabase_ReturnsExpected()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockFileRepository = new Mock<IBaseRepository<FileEntity>>();
            var mockLogRepository = new Mock<IBaseRepository<LogEntity>>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);
            var expectedDictionary = new Dictionary<string, string>
            {
                { "Sentiment", "Value" },
                { "Summary 1:", "Value" }
            };

            mockFileRepository.Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<FileEntity, bool>>>(),
                It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                .ReturnsAsync([]);

            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>()).Returns(mockFileRepository.Object);
            mockUnitOfWork.Setup(uow => uow.Repository<LogEntity>()).Returns(mockLogRepository.Object);
            mockAzureAIService.Setup(az => az.RunInvoiceAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync([]);
            mockAzureAIService.Setup(az => az.RunTextAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(expectedDictionary);

            // Act
            var result = await analyzerBusiness.Analyze(TEXT_BASE64);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDictionary, result.Data);
        }
    }
}
