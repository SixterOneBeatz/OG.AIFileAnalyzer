using OG.AIFileAnalyzer.Persistence.DataAccess.Repositories.BaseRepository;
using OG.AIFileAnalyzer.Persistence.Services.AzureAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OG.AIFileAnalyzer.Tests.AnalyzerBusinessTests
{
    public class CheckForExistingAnalysis : BaseTestClass
    {
        [Fact]
        public async Task CheckForExistingAnalysis_ExistingHash_ReturnsAnalysisResponseDTO()
        {
            // Arrange
            var hash = "existingHash";
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var azureAIMock = new Mock<IAzureAIService>();

            var analysisService = new AnalyzerBusiness(azureAIMock.Object, unitOfWorkMock.Object);

            var existingFileEntity = new FileEntity
            {
                SHA256 = hash,
                DocumentType = DocType.GeneralText,
                Anaysis = new List<FileAnaysisEntity>
                {
                    new FileAnaysisEntity { Key = "Sentiment", Value = "Positive" },
                    new FileAnaysisEntity { Key = "Summary", Value = "This is a test summary." }
                }
            };

            var expectedAnalysisResponse = new AnalysisResponseDTO
            {
                DocumentType = DocType.GeneralText,
                Data = new Dictionary<string, string>
                {
                    { "Sentiment", "Positive" },
                    { "Summary", "This is a test summary." }
                }
            };

            var files = new List<FileEntity> { existingFileEntity };

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<FileEntity, bool>>>(), It.IsAny<List<System.Linq.Expressions.Expression<System.Func<FileEntity, object>>>>()))
                .ReturnsAsync(files);

            // Act
            var result = await analysisService.CheckForExistingAnalysis(hash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAnalysisResponse.DocumentType, result.DocumentType);
            Assert.Equal(expectedAnalysisResponse.Data, result.Data);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_EmptyHash_ReturnsNull()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var analysisService = new AnalyzerBusiness(new Mock<IAzureAIService>().Object, unitOfWorkMock.Object);
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<FileEntity, bool>>>(), It.IsAny<List<System.Linq.Expressions.Expression<System.Func<FileEntity, object>>>>()))
                .ReturnsAsync([]);

            // Act
            var result = await analysisService.CheckForExistingAnalysis(string.Empty);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_NonExistingHash_ReturnsNull()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var analysisService = new AnalyzerBusiness(new Mock<IAzureAIService>().Object, unitOfWorkMock.Object);

            var hash = INVOICE_HASH;

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                             .ReturnsAsync([]);

            // Act
            var result = await analysisService.CheckForExistingAnalysis(hash);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_EmptyAnalysis_ReturnsEmptyAnalysisResponseDTO()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var analysisService = new AnalyzerBusiness(new Mock<IAzureAIService>().Object, unitOfWorkMock.Object);

            var files = new List<FileEntity> { new FileEntity { SHA256 = string.Empty, Anaysis = new List<FileAnaysisEntity>() } };

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                             .ReturnsAsync(files);

            // Act
            var result = await analysisService.CheckForExistingAnalysis(string.Empty);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_ValidHash_ReturnsAnalysisResponseDTO()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var analysisService = new AnalyzerBusiness(new Mock<IAzureAIService>().Object, unitOfWorkMock.Object);

            var hash = TEXT_HASH;

            var fileEntity = new FileEntity
            {
                DocumentType = DocType.GeneralText,
                SHA256 = hash,
                Anaysis = new List<FileAnaysisEntity>
                {
                    new FileAnaysisEntity { Key = "Key1", Value = "Value1" },
                    new FileAnaysisEntity { Key = "Key2", Value = "Value2" }
                }
            };

            var files = new List<FileEntity> { fileEntity };

            var expectedAnalysisResponse = new AnalysisResponseDTO
            {
                DocumentType = DocType.GeneralText,
                Data = new Dictionary<string, string>
                {
                    { "Key1", "Value1" },
                    { "Key2", "Value2" }
                }
            };

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                             .ReturnsAsync(files);

            // Act
            var result = await analysisService.CheckForExistingAnalysis(hash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAnalysisResponse.DocumentType, result.DocumentType);
            Assert.Equal(expectedAnalysisResponse.Data, result.Data);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_MultipleFilesWithSameHash_ReturnsFirstAnalysisResponseDTO()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var analysisService = new AnalyzerBusiness(new Mock<IAzureAIService>().Object, unitOfWorkMock.Object);

            var hash = INVOICE_HASH;

            var fileEntity1 = new FileEntity
            {
                DocumentType = DocType.GeneralText,
                SHA256 = hash,
                Anaysis = new List<FileAnaysisEntity> { new FileAnaysisEntity { Key = "Key1", Value = "Value1" } }
            };

            var fileEntity2 = new FileEntity
            {
                DocumentType = DocType.GeneralText,
                SHA256 = hash,
                Anaysis = new List<FileAnaysisEntity> { new FileAnaysisEntity { Key = "Key2", Value = "Value2" } }
            };

            var files = new List<FileEntity> { fileEntity1, fileEntity2 };

            var expectedAnalysisResponse = new AnalysisResponseDTO
            {
                DocumentType = DocType.GeneralText,
                Data = new Dictionary<string, string> { { "Key1", "Value1" } }
            };

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                             .ReturnsAsync(files);

            // Act
            var result = await analysisService.CheckForExistingAnalysis(hash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAnalysisResponse.DocumentType, result.DocumentType);
            Assert.Equal(expectedAnalysisResponse.Data, result.Data);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_MultipleFilesWithDifferentHashes_ReturnsNull()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var analysisService = new AnalyzerBusiness(new Mock<IAzureAIService>().Object, unitOfWorkMock.Object);


            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                             .ReturnsAsync([]);

            // Act
            var result = await analysisService.CheckForExistingAnalysis(string.Empty);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_ValidHashEmptyAnalysis_ReturnsEmptyAnalysis()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var analysisService = new AnalyzerBusiness(new Mock<IAzureAIService>().Object, unitOfWorkMock.Object);

            var hash = TEXT_HASH;

            var fileEntity = new FileEntity
            {
                SHA256 = hash,
                Anaysis = new List<FileAnaysisEntity>()
            };

            var files = new List<FileEntity> { fileEntity };

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                             .ReturnsAsync(files);

            // Act
            var result = await analysisService.CheckForExistingAnalysis(hash);

            // Assert
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_ValidHashWithNullAnalysisValue_ReturnsGeneralTextAnalysisResponseDTO()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var analysisService = new AnalyzerBusiness(new Mock<IAzureAIService>().Object, unitOfWorkMock.Object);

            var hash = "validHashWithNullAnalysisValue";

            var fileEntity = new FileEntity
            {
                DocumentType = DocType.GeneralText,
                SHA256 = hash,
                Anaysis = new List<FileAnaysisEntity>
                {
                    new FileAnaysisEntity { Key = "Key1", Value = "Value1" },
                    new FileAnaysisEntity { Key = "Key2", Value = null }
                }
            };

            var files = new List<FileEntity> { fileEntity };

            var expectedAnalysisResponse = new AnalysisResponseDTO
            {
                DocumentType = DocType.GeneralText,
                Data = new Dictionary<string, string>
                {
                    { "Key1", "Value1" },
                    { "Key2", null }
                }
            };

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                             .ReturnsAsync(files);

            // Act
            var result = await analysisService.CheckForExistingAnalysis(hash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAnalysisResponse.DocumentType, result.DocumentType);
            Assert.Equal(expectedAnalysisResponse.Data, result.Data);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_ValidHashWithNullAnalysisValue_ReturnsInvoiceAnalysisResponseDTO()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var fileRepositoryMock = new Mock<IBaseRepository<FileEntity>>();
            var analysisService = new AnalyzerBusiness(new Mock<IAzureAIService>().Object, unitOfWorkMock.Object);

            var hash = "validHashWithNullAnalysisValue";

            var fileEntity = new FileEntity
            {
                SHA256 = hash,
                Anaysis = new List<FileAnaysisEntity>
                {
                    new FileAnaysisEntity { Key = "Key1", Value = "Value1" },
                    new FileAnaysisEntity { Key = "Key2", Value = null }
                }
            };

            var files = new List<FileEntity> { fileEntity };

            var expectedAnalysisResponse = new AnalysisResponseDTO
            {
                Data = new Dictionary<string, string>
                {
                    { "Key1", "Value1" },
                    { "Key2", null }
                }
            };

            unitOfWorkMock.Setup(uow => uow.Repository<FileEntity>()).Returns(fileRepositoryMock.Object);
            fileRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<FileEntity, bool>>>(), It.IsAny<List<Expression<Func<FileEntity, object>>>>()))
                             .ReturnsAsync(files);

            // Act
            var result = await analysisService.CheckForExistingAnalysis(hash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAnalysisResponse.DocumentType, result.DocumentType);
            Assert.Equal(expectedAnalysisResponse.Data, result.Data);
        }
    }
}
