using OG.AIFileAnalyzer.Persistence.DataAccess.Repositories.BaseRepository;
using OG.AIFileAnalyzer.Persistence.Services.AzureAI;

namespace OG.AIFileAnalyzer.Tests.AnalyzerBusinessTests
{
    public class SaveAnalysis : BaseTestClass
    {
        [Fact]
        public async Task SaveAnalysis_ValidFile_CallsAddEntityAndComplete()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var azureAIMock = new Mock<IAzureAIService>();
            var analysisService = new AnalyzerBusiness(azureAIMock.Object, unitOfWorkMock.Object);

            var file = new AnalysisResponseDTO
            {
                DocumentType = DocType.Invoice,
                Data = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            }
            };
            string hash = INVOICE_HASH;

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);

            // Act
            await analysisService.SaveAnalysis(file, hash);

            // Assert
            fileRepositoryMock.Verify(repo => repo.AddEntity(It.IsAny<FileEntity>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact]
        public async Task SaveAnalysis_GeneralTextFile_CallsAddEntityAndComplete()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var azureAIMock = new Mock<IAzureAIService>();
            var analysisService = new AnalyzerBusiness(azureAIMock.Object, unitOfWorkMock.Object);

            var file = new AnalysisResponseDTO
            {
                DocumentType = DocType.GeneralText,
                Data = new Dictionary<string, string>
                {
                    { "Sentiment", "Positive" },
                    { "Summary", "This is a test summary." }
                }
            };
            string hash = TEXT_HASH;

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);

            // Act
            await analysisService.SaveAnalysis(file, hash);

            // Assert
            fileRepositoryMock.Verify(repo => repo.AddEntity(It.IsAny<FileEntity>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact]
        public async Task SaveAnalysis_ValidInvoiceFile_CallsAddEntityAndComplete_UniqueHash()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var azureAIMock = new Mock<IAzureAIService>();
            var analysisService = new AnalyzerBusiness(azureAIMock.Object, unitOfWorkMock.Object);

            var file = new AnalysisResponseDTO
            {
                DocumentType = DocType.Invoice,
                Data = new Dictionary<string, string>
        {
            { "Key1", "Value1" },
            { "Key2", "Value2" }
        }
            };
            string hash = "unique_invoice_hash";

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);

            // Act
            await analysisService.SaveAnalysis(file, hash);

            // Assert
            fileRepositoryMock.Verify(repo => repo.AddEntity(It.IsAny<FileEntity>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact]
        public async Task SaveAnalysis_ValidInvoiceFile_CallsAddEntityAndComplete_DuplicateHash()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var azureAIMock = new Mock<IAzureAIService>();
            var analysisService = new AnalyzerBusiness(azureAIMock.Object, unitOfWorkMock.Object);

            var file = new AnalysisResponseDTO
            {
                DocumentType = DocType.Invoice,
                Data = new Dictionary<string, string>
                {
                    { "Key1", "Value1" },
                    { "Key2", "Value2" }
                }
            };

            string hash = INVOICE_HASH;

            fileRepositoryMock.Setup(repo => repo.AddEntity(It.IsAny<FileEntity>())).Throws<Exception>();

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () => await analysisService.SaveAnalysis(file, hash));
        }

        [Fact]
        public async Task SaveAnalysis_ValidGeneralTextFile_CallsAddEntityAndComplete_UniqueHash()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var azureAIMock = new Mock<IAzureAIService>();
            var analysisService = new AnalyzerBusiness(azureAIMock.Object, unitOfWorkMock.Object);

            var file = new AnalysisResponseDTO
            {
                DocumentType = DocType.GeneralText,
                Data = new Dictionary<string, string>
                {
                    { "Sentiment", "Positive" },
                    { "Summary", "This is a test summary." }
                }
            };
            string hash = TEXT_HASH;

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);

            // Act
            await analysisService.SaveAnalysis(file, hash);

            // Assert
            fileRepositoryMock.Verify(repo => repo.AddEntity(It.IsAny<FileEntity>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact]
        public async Task SaveAnalysis_ValidGeneralTextFile_CallsAddEntityAndComplete_DuplicateHash()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var azureAIMock = new Mock<IAzureAIService>();
            var analysisService = new AnalyzerBusiness(azureAIMock.Object, unitOfWorkMock.Object);

            var file = new AnalysisResponseDTO
            {
                DocumentType = DocType.GeneralText,
                Data = new Dictionary<string, string>
                {
                    { "Sentiment", "Positive" },
                    { "Summary", "This is a test summary." }
                }
            };
            string hash = TEXT_HASH; // Assuming this hash already exists in the database

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.AddEntity(It.IsAny<FileEntity>())).Throws<Exception>();

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () => await analysisService.SaveAnalysis(file, hash));
        }

        [Fact]
        public async Task SaveAnalysis_EmptyData_ThrowsArgumentException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var analysisService = new AnalyzerBusiness(new Mock<IAzureAIService>().Object, unitOfWorkMock.Object);
            var file = new AnalysisResponseDTO
            {
                DocumentType = DocType.Invoice,
                Data = new Dictionary<string, string>()
            };
            string hash = INVOICE_HASH;

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.AddEntity(It.IsAny<FileEntity>())).Throws<ArgumentException>();

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await analysisService.SaveAnalysis(file, hash));
        }

        [Fact]
        public async Task SaveAnalysis_ValidFile_CallsAddEntityAndComplete_GeneralTextFile()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var azureAIMock = new Mock<IAzureAIService>();
            var analysisService = new AnalyzerBusiness(azureAIMock.Object, unitOfWorkMock.Object);

            var file = new AnalysisResponseDTO
            {
                DocumentType = DocType.GeneralText,
                Data = new Dictionary<string, string>
                {
                    { "Sentiment", "Positive" },
                    { "Summary", "This is a test summary." }
                }
            };
            string hash = TEXT_HASH;

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.AddEntity(It.IsAny<FileEntity>())).Returns(Task.CompletedTask);

            // Act
            await analysisService.SaveAnalysis(file, hash);

            // Assert
            fileRepositoryMock.Verify(repo => repo.AddEntity(It.IsAny<FileEntity>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact]
        public async Task SaveAnalysis_EmptyFile_DoesNotCallAddEntityAndComplete()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var azureAIMock = new Mock<IAzureAIService>();
            var analysisService = new AnalyzerBusiness(azureAIMock.Object, unitOfWorkMock.Object);

            string hash = TEXT_HASH;

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.AddEntity(It.IsAny<FileEntity>())).Returns(Task.CompletedTask);

            // Act
            await analysisService.SaveAnalysis(null, hash);

            // Assert
            fileRepositoryMock.Verify(repo => repo.AddEntity(It.IsAny<FileEntity>()), Times.Never);
            unitOfWorkMock.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact]
        public async Task SaveAnalysis_ValidFile_CallsAddEntityAndComplete_InvoiceFile()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var azureAIMock = new Mock<IAzureAIService>();
            var analysisService = new AnalyzerBusiness(azureAIMock.Object, unitOfWorkMock.Object);

            var file = new AnalysisResponseDTO
            {
                DocumentType = DocType.Invoice,
                Data = new Dictionary<string, string>
                {
                    { "InvoiceId", "123456" },
                    { "InvoiceTotal", "500" }
                }
            };
            string hash = "invoiceFileHash";

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.AddEntity(It.IsAny<FileEntity>())).Returns(Task.CompletedTask);

            // Act
            await analysisService.SaveAnalysis(file, hash);

            // Assert
            fileRepositoryMock.Verify(repo => repo.AddEntity(It.IsAny<FileEntity>()), Times.Once);
            unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
        }
    }
}
