using OG.AIFileAnalyzer.Persistence.Services.AzureAI;
using System.Linq.Expressions;


namespace OG.AIFileAnalyzer.Tests.AnalyzerBusinessTests
{
    public class Analyze : BaseTestClass
    {
        #region Analyze Test Cases
        [Fact]
        public async Task Analyze_WithExistingAnalysisInDatabase_ReturnsExistingAnalysis()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);

            var existingHash = EXISTING_INVOICE_HASH;
            var existingAnalysis = new AnalysisResponseDTO { /* Mock existing analysis */ };

            // Setup predicate for GetAsync method
            Expression<Func<FileEntity, bool>> predicate = It.IsAny<Expression<Func<FileEntity, bool>>>();

            // Setup includes for GetAsync method
            List<Expression<Func<FileEntity, object>>> includes = It.IsAny<List<Expression<Func<FileEntity, object>>>>();

            // Setup mockUnitOfWork to return a mock result for GetAsync method
            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>().GetAsync(predicate, includes))
                          .ReturnsAsync(new List<FileEntity> { new FileEntity { SHA256 = existingHash, Anaysis = new List<FileAnaysisEntity>() } });

            // Act
            var result = await analyzerBusiness.Analyze(existingHash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingAnalysis, result);
        }


        [Fact]
        public async Task Analyze_WithNonExistingAnalysisInDatabase_ReturnsNewAnalysis()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);

            var nonExistingHash = "nonExistingHash";
            var newAnalysis = new AnalysisResponseDTO { /* Mock new analysis */ };

            // Setup predicate for GetAsync method
            Expression<Func<FileEntity, bool>> predicate = It.IsAny<Expression<Func<FileEntity, bool>>>();

            // Setup includes for GetAsync method
            List<Expression<Func<FileEntity, object>>> includes = It.IsAny<List<Expression<Func<FileEntity, object>>>>();

            // Setup mockUnitOfWork to return a mock result for GetAsync method
            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>().GetAsync(predicate, includes))
                          .ReturnsAsync(new List<FileEntity> { new FileEntity { SHA256 = nonExistingHash, Anaysis = new List<FileAnaysisEntity>() } });

            mockAzureAIService.Setup(service => service.RunInvoiceAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(new Dictionary<string, string>());
            mockAzureAIService.Setup(service => service.RunTextAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(new Dictionary<string, string>());

            // Act
            var result = await analyzerBusiness.Analyze(nonExistingHash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newAnalysis.Data, result.Data);
            Assert.Equal(newAnalysis.DocumentType, result.DocumentType);
        }

        [Fact]
        public async Task Analyze_WithInvoiceContent_ReturnsInvoiceAnalysis()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);

            var invoiceHash = "invoiceHash";
            var invoiceContent = "invoiceContent";
            var invoiceData = new Dictionary<string, string> { { "InvoiceId", "123" }, { "InvoiceTotal", "100" } };

            mockAzureAIService.Setup(service => service.RunInvoiceAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(invoiceData);
            mockAzureAIService.Setup(service => service.RunTextAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(new Dictionary<string, string>());

            // Act
            var result = await analyzerBusiness.Analyze(invoiceContent);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(invoiceData, result.Data);
            Assert.Equal(DocType.Invoice, result.DocumentType);
        }

        [Fact]
        public async Task Analyze_WithGeneralTextContent_ReturnsTextAnalysis()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);

            var textHash = "textHash";
            var textContent = "textContent";
            var textData = new Dictionary<string, string> { { "Key1", "Value1" }, { "Key2", "Value2" } };

            mockAzureAIService.Setup(service => service.RunInvoiceAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(new Dictionary<string, string>());
            mockAzureAIService.Setup(service => service.RunTextAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(textData);

            // Act
            var result = await analyzerBusiness.Analyze(textContent);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(textData, result.Data);
            Assert.Equal(DocType.GeneralText, result.DocumentType);
        }

        [Fact]
        public async Task Analyze_WithEmptyContent_ThrowsException()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await analyzerBusiness.Analyze(string.Empty));
        }

        [Fact]
        public async Task Analyze_WithNullContent_ThrowsException()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await analyzerBusiness.Analyze(null));
        }

        [Fact]
        public async Task Analyze_WithInvalidBase64Content_ThrowsException()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);

            var invalidBase64Content = "invalidBase64Content";

            // Act & Assert
            await Assert.ThrowsAsync<FormatException>(async () => await analyzerBusiness.Analyze(invalidBase64Content));
        }

        [Fact]
        public async Task Analyze_WithInvoiceContent_StoresAnalysisInDatabase()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);

            var invoiceHash = "invoiceHash";
            var invoiceContent = "invoiceContent";
            var invoiceData = new Dictionary<string, string> { { "InvoiceId", "123" }, { "InvoiceTotal", "100" } };

            mockAzureAIService.Setup(service => service.RunInvoiceAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(invoiceData);
            mockAzureAIService.Setup(service => service.RunTextAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(new Dictionary<string, string>());

            // Act
            await analyzerBusiness.Analyze(invoiceContent);

            // Assert
            mockUnitOfWork.Verify(uow => uow.Repository<LogEntity>().AddEntity(It.IsAny<LogEntity>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact]
        public async Task Analyze_WithGeneralTextContent_StoresAnalysisInDatabase()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);

            var textHash = "textHash";
            var textContent = "textContent";
            var textData = new Dictionary<string, string> { { "Key1", "Value1" }, { "Key2", "Value2" } };

            mockAzureAIService.Setup(service => service.RunInvoiceAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(new Dictionary<string, string>());
            mockAzureAIService.Setup(service => service.RunTextAnalysis(It.IsAny<MemoryStream>())).ReturnsAsync(textData);

            // Act
            await analyzerBusiness.Analyze(textContent);

            // Assert
            mockUnitOfWork.Verify(uow => uow.Repository<LogEntity>().AddEntity(It.IsAny<LogEntity>()), Times.Once);
            mockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact]
        public async Task Analyze_Always_CompletesUnitOfWork()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockAzureAIService = new Mock<IAzureAIService>();
            var analyzerBusiness = new AnalyzerBusiness(mockAzureAIService.Object, mockUnitOfWork.Object);

            // Act
            await analyzerBusiness.Analyze("content");

            // Assert
            mockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }
        #endregion
    }
}