using System.Linq.Expressions;

namespace OG.AIFileAnalyzer.Tests.AnalyzerBusinessTests
{
    public class CheckForExistingAnalysis : BaseTestClass
    {
        #region CheckForExistingAnalysis Test Cases
        [Fact]
        public async Task CheckForExistingAnalysis_WithExistingAnalysis_ReturnsAnalysis()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var analyzerBusiness = new AnalyzerBusiness(null, mockUnitOfWork.Object);

            var existingHash = "existingHash";
            var existingAnalysis = new AnalysisResponseDTO { /* Mock existing analysis */ };

            // Setup predicate for GetAsync method
            Expression<Func<FileEntity, bool>> predicate = It.IsAny<Expression<Func<FileEntity, bool>>>();

            // Setup includes for GetAsync method
            List<Expression<Func<FileEntity, object>>> includes = It.IsAny<List<Expression<Func<FileEntity, object>>>>();

            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>().GetAsync(predicate, includes))
                          .ReturnsAsync(new List<FileEntity> { new FileEntity { SHA256 = existingHash, Anaysis = new List<FileAnaysisEntity>() } });

            // Act
            var result = await analyzerBusiness.CheckForExistingAnalysis(existingHash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingAnalysis, result);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_WithNonExistingAnalysis_ReturnsNull()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var analyzerBusiness = new AnalyzerBusiness(null, mockUnitOfWork.Object);

            var nonExistingHash = "nonExistingHash";

            // Setup predicate for GetAsync method
            Expression<Func<FileEntity, bool>> predicate = It.IsAny<Expression<Func<FileEntity, bool>>>();

            // Setup includes for GetAsync method
            List<Expression<Func<FileEntity, object>>> includes = It.IsAny<List<Expression<Func<FileEntity, object>>>>();

            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>().GetAsync(predicate, includes))
                          .ReturnsAsync(new List<FileEntity>());

            // Act
            var result = await analyzerBusiness.CheckForExistingAnalysis(nonExistingHash);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_WithEmptyHash_ReturnsNull()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var analyzerBusiness = new AnalyzerBusiness(null, mockUnitOfWork.Object);

            string emptyHash = string.Empty;

            // Act
            var result = await analyzerBusiness.CheckForExistingAnalysis(emptyHash);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_WithNullHash_ReturnsNull()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var analyzerBusiness = new AnalyzerBusiness(null, mockUnitOfWork.Object);

            string? nullHash = null;

            // Act
            var result = await analyzerBusiness.CheckForExistingAnalysis(nullHash);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_WithInvalidHash_ReturnsNull()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var analyzerBusiness = new AnalyzerBusiness(null, mockUnitOfWork.Object);

            var invalidHash = "invalidHash";

            // Setup predicate for GetAsync method
            Expression<Func<FileEntity, bool>> predicate = It.IsAny<Expression<Func<FileEntity, bool>>>();

            // Setup includes for GetAsync method
            List<Expression<Func<FileEntity, object>>> includes = It.IsAny<List<Expression<Func<FileEntity, object>>>>();

            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>().GetAsync(predicate, includes))
                          .ReturnsAsync((List<FileEntity>?)null); // Simulating invalid hash

            // Act
            var result = await analyzerBusiness.CheckForExistingAnalysis(invalidHash);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_WithMultipleAnalysis_ReturnsFirstMatch()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var analyzerBusiness = new AnalyzerBusiness(null, mockUnitOfWork.Object);

            var existingHash = "existingHash";
            var existingAnalysis = new AnalysisResponseDTO { /* Mock existing analysis */ };

            // Setup predicate for GetAsync method
            Expression<Func<FileEntity, bool>> predicate = It.IsAny<Expression<Func<FileEntity, bool>>>();

            // Setup includes for GetAsync method
            List<Expression<Func<FileEntity, object>>> includes = It.IsAny<List<Expression<Func<FileEntity, object>>>>();

            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>().GetAsync(predicate, includes))
                          .ReturnsAsync(new List<FileEntity>
                          {
                              new FileEntity { SHA256 = existingHash, Anaysis = new List<FileAnaysisEntity>() },
                              new FileEntity { SHA256 = "anotherHash", Anaysis = new List<FileAnaysisEntity>() }
                          });

            // Act
            var result = await analyzerBusiness.CheckForExistingAnalysis(existingHash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingAnalysis, result);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_WithSpecificDocumentType_ReturnsMatchingAnalysis()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var analyzerBusiness = new AnalyzerBusiness(null, mockUnitOfWork.Object);

            var existingHash = "existingHash";
            var existingAnalysis = new AnalysisResponseDTO { /* Mock existing analysis */ };

            // Setup predicate for GetAsync method
            Expression<Func<FileEntity, bool>> predicate = It.IsAny<Expression<Func<FileEntity, bool>>>();

            // Setup includes for GetAsync method
            List<Expression<Func<FileEntity, object>>> includes = It.IsAny<List<Expression<Func<FileEntity, object>>>>();

            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>().GetAsync(predicate, includes))
                          .ReturnsAsync(new List<FileEntity>
                          {
                              new FileEntity { SHA256 = existingHash, Anaysis = new List<FileAnaysisEntity>() { /* Mock analysis with specific document type */ } },
                              new FileEntity { SHA256 = "anotherHash", Anaysis = new List<FileAnaysisEntity>() }
                          });

            // Act
            var result = await analyzerBusiness.CheckForExistingAnalysis(existingHash);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingAnalysis, result);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_WithSpecificDocumentType_NoMatch_ReturnsNull()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var analyzerBusiness = new AnalyzerBusiness(null, mockUnitOfWork.Object);

            var existingHash = "existingHash";

            // Setup predicate for GetAsync method
            Expression<Func<FileEntity, bool>> predicate = It.IsAny<Expression<Func<FileEntity, bool>>>();

            // Setup includes for GetAsync method
            List<Expression<Func<FileEntity, object>>> includes = It.IsAny<List<Expression<Func<FileEntity, object>>>>();

            // Setup mockUnitOfWork to return a mock result for GetAsync method
            mockUnitOfWork.Setup(uow => uow.Repository<FileEntity>().GetAsync(predicate, includes))
                          .ReturnsAsync(new List<FileEntity>
                          {
                              new FileEntity { SHA256 = existingHash, Anaysis = new List<FileAnaysisEntity>() { /* Mock analysis with specific document type */ } },
                              new FileEntity { SHA256 = "anotherHash", Anaysis = new List<FileAnaysisEntity>() }
                          });

            // Act
            var result = await analyzerBusiness.CheckForExistingAnalysis(existingHash);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CheckForExistingAnalysis_WithNullUnitOfWork_ThrowsException()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var analyzerBusiness = new AnalyzerBusiness(null, null); // Passing null unit of work intentionally

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await analyzerBusiness.CheckForExistingAnalysis("hash"));
        }

        [Fact]
        public async Task CheckForExistingAnalysis_Always_CompletesUnitOfWork()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var analyzerBusiness = new AnalyzerBusiness(null, mockUnitOfWork.Object);

            // Act
            await analyzerBusiness.CheckForExistingAnalysis("hash");

            // Assert
            mockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }
        #endregion
    }
}
