using FluentAssertions;
using Hotels.Application.Dtos.Reviews;
using Hotels.Domain.Entities.Reviews;
using Hotels.PartnerReviews.Controllers;
using Hotels.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace Hotels.Integration.Tests;

public class PartnerReviewsInternalControllerTests : IClassFixture<WebApplicationFactory<PartnerReviewsInternalController>>
{
    private readonly string GetDtosIncludedUrl = "GetDtosIncluded/";
    private readonly string GetDtoIncludedAsyncUrl = "GetDtoIncluded/";
    private readonly string GetDtosIncludedByPartnerUrl = "GetDtosIncludedByPartner/";
    private readonly string GetDtosIncludedByTouristUrl = "GetDtosIncludedByTourist/";

    private readonly Uri _baseAddress = new("https://localhost:5008/api/internal/v1/partner_reviews/");

    private readonly WebApplicationFactory<PartnerReviewsInternalController> _factory;

    public PartnerReviewsInternalControllerTests(WebApplicationFactory<PartnerReviewsInternalController> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptors = services.Where(d =>
                        d.ServiceType == typeof(DbContextOptions<ApplicationContext>)
                     || d.ServiceType == typeof(IDbContextOptionsConfiguration<ApplicationContext>))
                    .ToArray();

                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }

                string dbName = Guid.NewGuid().ToString();
                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseInMemoryDatabase(dbName);
                });
            });
        });
    }

    #region GetDtosIncluded
    [Fact]
    public async Task GetDtosIncluded_Succeed()
    {
        // Arrange
        Guid reviewId1 = Guid.NewGuid();
        Guid reviewId2 = Guid.NewGuid();

        Guid imageLinkId1 = Guid.NewGuid();
        Guid imageLinkId2 = Guid.NewGuid();
        Guid imageLinkId3 = Guid.NewGuid();
        Guid imageLinkId4 = Guid.NewGuid();

        List<PartnerReview> testReviews =
        [
            new()
            {
                Id = reviewId1,
                Name = "Name 1",
                PartnerId = "partner-1",
                TouristId = "tourist-1",
                Rating = 0.3f,
                Comment = "Comment 1",
                CreatedAt = DateTime.MinValue,
                ImageLinks =
                [
                    new() { IsTitle = false, Uri = "uri1", ReviewId = reviewId1, Id = imageLinkId1 },
                    new() { IsTitle = false, Uri = "uri2", ReviewId = reviewId1, Id = imageLinkId2 }
                ]
            },
            new()
            {
                Id = reviewId2,
                Name = "Name 2",
                PartnerId = "partner-1",
                TouristId = "tourist-2",
                Rating = 5,
                Comment = "Comment 2",
                CreatedAt = DateTime.MaxValue,
                ImageLinks =
                [
                    new() { IsTitle = true, Uri = "uri3", ReviewId = reviewId2, Id = imageLinkId3 },
                    new() { IsTitle = false, Uri = "uri4", ReviewId = reviewId2, Id = imageLinkId4 }
                ]
            }
        ];

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            await db.ObjectReviews.AddRangeAsync(testReviews);
            await db.SaveChangesAsync();
        }

        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        // Act
        var response = await client.GetAsync(GetDtosIncludedUrl);
        var rawResult = await response.Content.ReadFromJsonAsync<IEnumerable<PartnerReviewDto>>();
        var result = rawResult?.ToList();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        result[0].Should().BeEquivalentTo(new PartnerReviewDto
        {
            Id = reviewId1,
            Name = "Name 1",
            PartnerId = "partner-1",
            TouristId = "tourist-1",
            Rating = 0.3f,
            Comment = "Comment 1",
            CreatedAt = DateTime.MinValue,
            ImageLinks =
            [
                new() { IsTitle = false, Uri = "uri1", Id = imageLinkId1 },
                new() { IsTitle = false, Uri = "uri2", Id = imageLinkId2 }
            ]
        });
        result[1].Should().BeEquivalentTo(new PartnerReviewDto
        {
            Id = reviewId2,
            Name = "Name 2",
            PartnerId = "partner-1",
            TouristId = "tourist-2",
            Rating = 5,
            Comment = "Comment 2",
            CreatedAt = DateTime.MaxValue,
            ImageLinks =
            [
                new() { IsTitle = true, Uri = "uri3", Id = imageLinkId3 },
                new() { IsTitle = false, Uri = "uri4", Id = imageLinkId4 }
            ]
        });
    }

    [Fact]
    public async Task GetDtosIncluded_WhenNoReviews_ReturnsEmpty()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        // Act
        var response = await client.GetAsync(GetDtosIncludedUrl);
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<PartnerReviewDto>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    #endregion

    #region GetDtoIncludedAsync
    [Fact]
    public async Task GetDtoIncludedAsync_Succeed()
    {
        // Arrange
        Guid reviewId1 = Guid.NewGuid();
        Guid reviewId2 = Guid.NewGuid();

        Guid imageLinkId1 = Guid.NewGuid();
        Guid imageLinkId2 = Guid.NewGuid();
        Guid imageLinkId3 = Guid.NewGuid();
        Guid imageLinkId4 = Guid.NewGuid();

        List<PartnerReview> testReviews =
        [
            new()
            {
                Id = reviewId1,
                Name = "Name 1",
                PartnerId = "partner-1",
                TouristId = "tourist-1",
                Rating = 0.3f,
                Comment = "Comment 1",
                CreatedAt = DateTime.MinValue,
                ImageLinks =
                [
                    new() {IsTitle = false, Uri = "uri1", ReviewId = reviewId1, Id = imageLinkId1 },
                    new() {IsTitle = false, Uri = "uri2", ReviewId = reviewId1, Id = imageLinkId2 }
                ]
            },
            new()
            {
                Id = reviewId2,
                Name = "Name 2",
                PartnerId = "partner-1",
                TouristId = "tourist-2",
                Rating = 5,
                Comment = "Comment 2",
                CreatedAt = DateTime.MaxValue,
                ImageLinks =
                [
                    new() { IsTitle = true, Uri = "uri3", ReviewId = reviewId2, Id = imageLinkId3 },
                    new() { IsTitle = false, Uri = "uri4", ReviewId = reviewId2, Id = imageLinkId4 }
                ]
            }
        ];

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            await db.ObjectReviews.AddRangeAsync(testReviews);
            await db.SaveChangesAsync();
        }

        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        // Act
        var response = await client.GetAsync(GetDtoIncludedAsyncUrl + reviewId1);
        var result = await response.Content.ReadFromJsonAsync<PartnerReviewDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();

        result.Should().BeEquivalentTo(new PartnerReviewDto
        {
            Id = reviewId1,
            Name = "Name 1",
            PartnerId = "partner-1",
            TouristId = "tourist-1",
            Rating = 0.3f,
            Comment = "Comment 1",
            CreatedAt = DateTime.MinValue,
            ImageLinks =
            [
                new() {IsTitle = false, Uri = "uri1", Id = imageLinkId1 },
                new() {IsTitle = false, Uri = "uri2", Id = imageLinkId2 }
            ]
        });
    }

    [Fact]
    public async Task GetDtoIncludedAsync_WithNotExistingReview_Returns404()
    {
        // Arrange
        Guid reviewId1 = Guid.NewGuid();
        Guid reviewId2 = Guid.NewGuid();

        Guid imageLinkId1 = Guid.NewGuid();
        Guid imageLinkId2 = Guid.NewGuid();
        Guid imageLinkId3 = Guid.NewGuid();
        Guid imageLinkId4 = Guid.NewGuid();

        List<PartnerReview> testReviews =
        [
            new()
            {
                Id = reviewId1,
                Name = "Name 1",
                PartnerId = "partner-1",
                TouristId = "tourist-1",
                Rating = 0.3f,
                Comment = "Comment 1",
                CreatedAt = DateTime.MinValue,
                ImageLinks =
                [
                    new() {IsTitle = false, Uri = "uri1", ReviewId = reviewId1, Id = imageLinkId1 },
                    new() {IsTitle = false, Uri = "uri2", ReviewId = reviewId1, Id = imageLinkId2 }
                ]
            },
            new()
            {
                Id = reviewId2,
                Name = "Name 2",
                PartnerId = "partner-1",
                TouristId = "tourist-2",
                Rating = 5,
                Comment = "Comment 2",
                CreatedAt = DateTime.MaxValue,
                ImageLinks =
                [
                    new() { IsTitle = true, Uri = "uri3", ReviewId = reviewId2, Id = imageLinkId3 },
                    new() { IsTitle = false, Uri = "uri4", ReviewId = reviewId2, Id = imageLinkId4 }
                ]
            }
        ];

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            await db.ObjectReviews.AddRangeAsync(testReviews);
            await db.SaveChangesAsync();
        }

        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        // Act
        var response = await client.GetAsync(GetDtoIncludedAsyncUrl + Guid.NewGuid());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    #endregion

    #region GetDtosIncludedByPartner
    [Fact]
    public async Task GetDtosIncludedByPartner_Succeed()
    {
        // Arrange
        Guid reviewId1 = Guid.NewGuid();
        Guid reviewId2 = Guid.NewGuid();
        Guid reviewId3 = Guid.NewGuid();

        Guid imageLinkId1 = Guid.NewGuid();
        Guid imageLinkId2 = Guid.NewGuid();
        Guid imageLinkId3 = Guid.NewGuid();
        Guid imageLinkId4 = Guid.NewGuid();
        Guid imageLinkId5 = Guid.NewGuid();
        Guid imageLinkId6 = Guid.NewGuid();

        string partnerId1 = Guid.NewGuid().ToString();

        List<PartnerReview> testReviews =
        [
            new()
            {
                Id = reviewId1,
                Name = "Name 1",
                PartnerId = partnerId1,
                TouristId = "tourist-1",
                Rating = 0.3f,
                Comment = "Comment 1",
                CreatedAt = DateTime.MinValue,
                ImageLinks =
                [
                    new() { IsTitle = false, Uri = "uri1", ReviewId = reviewId1, Id = imageLinkId1 },
                    new() { IsTitle = false, Uri = "uri2", ReviewId = reviewId1, Id = imageLinkId2 }
                ]
            },
            new()
            {
                Id = reviewId2,
                Name = "Name 2",
                PartnerId = partnerId1,
                TouristId = "tourist-2",
                Rating = 5,
                Comment = "Comment 2",
                CreatedAt = DateTime.MaxValue,
                ImageLinks =
                [
                    new() { IsTitle = true, Uri = "uri3", ReviewId = reviewId2, Id = imageLinkId3 },
                    new() { IsTitle = false, Uri = "uri4", ReviewId = reviewId2, Id = imageLinkId4 }
                ]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Name 3",
                PartnerId = Guid.NewGuid().ToString(),
                TouristId = "tourist-3",
                Rating = 1,
                Comment = "Comment 3",
                CreatedAt = DateTime.MaxValue,
                ImageLinks =
                [
                    new() { IsTitle = true, Uri = "uri5", ReviewId = reviewId3, Id = imageLinkId5 },
                    new() { IsTitle = false, Uri = "uri6", ReviewId = reviewId3, Id = imageLinkId6 }
                ]
            }
        ];

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            await db.ObjectReviews.AddRangeAsync(testReviews);
            await db.SaveChangesAsync();
        }

        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        // Act
        var response = await client.GetAsync(GetDtosIncludedByPartnerUrl + partnerId1);
        var rawResult = await response.Content.ReadFromJsonAsync<IEnumerable<PartnerReviewDto>>();
        var result = rawResult?.ToList();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().HaveCount(2);

        result[0].Should().BeEquivalentTo(new PartnerReviewDto
        {
            Id = reviewId1,
            Name = "Name 1",
            PartnerId = partnerId1,
            TouristId = "tourist-1",
            Rating = 0.3f,
            Comment = "Comment 1",
            CreatedAt = DateTime.MinValue,
            ImageLinks =
            [
                new() { IsTitle = false, Uri = "uri1", Id = imageLinkId1 },
                new() { IsTitle = false, Uri = "uri2", Id = imageLinkId2 }
            ]
        });
        result[1].Should().BeEquivalentTo(new PartnerReviewDto
        {
            Id = reviewId2,
            Name = "Name 2",
            PartnerId = partnerId1,
            TouristId = "tourist-2",
            Rating = 5,
            Comment = "Comment 2",
            CreatedAt = DateTime.MaxValue,
            ImageLinks =
            [
                new() { IsTitle = true, Uri = "uri3", Id = imageLinkId3 },
                new() { IsTitle = false, Uri = "uri4", Id = imageLinkId4 }
            ]
        });
    }

    [Fact]
    public async Task GetDtosIncludedByPartner_WithNotExistingPartnerId_ReturnsEmptyWith200()
    {
        // Arrange
        Guid reviewId1 = Guid.NewGuid();
        Guid reviewId2 = Guid.NewGuid();

        Guid imageLinkId1 = Guid.NewGuid();
        Guid imageLinkId2 = Guid.NewGuid();
        Guid imageLinkId3 = Guid.NewGuid();
        Guid imageLinkId4 = Guid.NewGuid();

        List<PartnerReview> testReviews =
        [
            new()
            {
                Id = reviewId1,
                Name = "Name 1",
                PartnerId = "partner-1",
                TouristId = "tourist-1",
                Rating = 0.3f,
                Comment = "Comment 1",
                CreatedAt = DateTime.MinValue,
                ImageLinks =
                [
                    new() {IsTitle = false, Uri = "uri1", ReviewId = reviewId1, Id = imageLinkId1 },
                    new() {IsTitle = false, Uri = "uri2", ReviewId = reviewId1, Id = imageLinkId2 }
                ]
            },
            new()
            {
                Id = reviewId2,
                Name = "Name 2",
                PartnerId = "partner-1",
                TouristId = "tourist-2",
                Rating = 5,
                Comment = "Comment 2",
                CreatedAt = DateTime.MaxValue,
                ImageLinks =
                [
                    new() { IsTitle = true, Uri = "uri3", ReviewId = reviewId2, Id = imageLinkId3 },
                    new() { IsTitle = false, Uri = "uri4", ReviewId = reviewId2, Id = imageLinkId4 }
                ]
            }
        ];

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            await db.ObjectReviews.AddRangeAsync(testReviews);
            await db.SaveChangesAsync();
        }

        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        // Act
        var response = await client.GetAsync(GetDtosIncludedByPartnerUrl + Guid.NewGuid());
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<PartnerReviewDto>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeEmpty();
    }
    #endregion

    #region GetDtosIncludedByTourist
    [Fact]
    public async Task GetDtosIncludedByTourist_Succeed()
    {
        // Arrange
        Guid reviewId1 = Guid.NewGuid();
        Guid reviewId2 = Guid.NewGuid();
        Guid reviewId3 = Guid.NewGuid();

        Guid imageLinkId1 = Guid.NewGuid();
        Guid imageLinkId2 = Guid.NewGuid();
        Guid imageLinkId3 = Guid.NewGuid();
        Guid imageLinkId4 = Guid.NewGuid();
        Guid imageLinkId5 = Guid.NewGuid();
        Guid imageLinkId6 = Guid.NewGuid();

        string touristId1 = Guid.NewGuid().ToString();
        string touristId2 = Guid.NewGuid().ToString();

        List<PartnerReview> testReviews =
        [
            new()
            {
                Id = reviewId1,
                Name = "Name 1",
                PartnerId = "partner-1",
                TouristId = touristId1,
                Rating = 0.3f,
                Comment = "Comment 1",
                CreatedAt = DateTime.MinValue,
                ImageLinks =
                [
                    new() { IsTitle = false, Uri = "uri1", ReviewId = reviewId1, Id = imageLinkId1 },
                    new() { IsTitle = false, Uri = "uri2", ReviewId = reviewId1, Id = imageLinkId2 }
                ]
            },
            new()
            {
                Id = reviewId2,
                Name = "Name 2",
                PartnerId = "partner-2",
                TouristId = touristId1,
                Rating = 5,
                Comment = "Comment 2",
                CreatedAt = DateTime.MaxValue,
                ImageLinks =
                [
                    new() { IsTitle = true, Uri = "uri3", ReviewId = reviewId2, Id = imageLinkId3 },
                    new() { IsTitle = false, Uri = "uri4", ReviewId = reviewId2, Id = imageLinkId4 }
                ]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Name 3",
                PartnerId = "partner-3",
                TouristId = touristId2,
                Rating = 1,
                Comment = "Comment 3",
                CreatedAt = DateTime.MaxValue,
                ImageLinks =
                [
                    new() { IsTitle = true, Uri = "uri5", ReviewId = reviewId3, Id = imageLinkId5 },
                    new() { IsTitle = false, Uri = "uri6", ReviewId = reviewId3, Id = imageLinkId6 }
                ]
            }
        ];

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            await db.ObjectReviews.AddRangeAsync(testReviews);
            await db.SaveChangesAsync();
        }

        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        // Act
        var response = await client.GetAsync(GetDtosIncludedByTouristUrl + touristId1);
        var rawResult = await response.Content.ReadFromJsonAsync<IEnumerable<PartnerReviewDto>>();
        var result = rawResult?.ToList();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().HaveCount(2);

        result[0].Should().BeEquivalentTo(new PartnerReviewDto
        {
            Id = reviewId1,
            Name = "Name 1",
            PartnerId = "partner-1",
            TouristId = touristId1,
            Rating = 0.3f,
            Comment = "Comment 1",
            CreatedAt = DateTime.MinValue,
            ImageLinks =
            [
                new() { IsTitle = false, Uri = "uri1", Id = imageLinkId1 },
                new() { IsTitle = false, Uri = "uri2", Id = imageLinkId2 }
            ]
        });
        result[1].Should().BeEquivalentTo(new PartnerReviewDto
        {
            Id = reviewId2,
            Name = "Name 2",
            PartnerId = "partner-2",
            TouristId = touristId1,
            Rating = 5,
            Comment = "Comment 2",
            CreatedAt = DateTime.MaxValue,
            ImageLinks =
            [
                new() { IsTitle = true, Uri = "uri3", Id = imageLinkId3 },
                new() { IsTitle = false, Uri = "uri4", Id = imageLinkId4 }
            ]
        });
    }

    [Fact]
    public async Task GetDtosIncludedByTourist_WithNotExistingTouristId_ReturnsEmptyWith200()
    {
        // Arrange
        Guid reviewId1 = Guid.NewGuid();
        Guid reviewId2 = Guid.NewGuid();
        Guid reviewId3 = Guid.NewGuid();

        Guid imageLinkId1 = Guid.NewGuid();
        Guid imageLinkId2 = Guid.NewGuid();
        Guid imageLinkId3 = Guid.NewGuid();
        Guid imageLinkId4 = Guid.NewGuid();
        Guid imageLinkId5 = Guid.NewGuid();
        Guid imageLinkId6 = Guid.NewGuid();

        string touristId1 = Guid.NewGuid().ToString();
        string touristId2 = Guid.NewGuid().ToString();

        List<PartnerReview> testReviews =
        [
            new()
            {
                Id = reviewId1,
                Name = "Name 1",
                PartnerId = "partner-1",
                TouristId = touristId1,
                Rating = 0.3f,
                Comment = "Comment 1",
                CreatedAt = DateTime.MinValue,
                ImageLinks =
                [
                    new() { IsTitle = false, Uri = "uri1", ReviewId = reviewId1, Id = imageLinkId1 },
                    new() { IsTitle = false, Uri = "uri2", ReviewId = reviewId1, Id = imageLinkId2 }
                ]
            },
            new()
            {
                Id = reviewId2,
                Name = "Name 2",
                PartnerId = "partner-2",
                TouristId = touristId1,
                Rating = 5,
                Comment = "Comment 2",
                CreatedAt = DateTime.MaxValue,
                ImageLinks =
                [
                    new() { IsTitle = true, Uri = "uri3", ReviewId = reviewId2, Id = imageLinkId3 },
                    new() { IsTitle = false, Uri = "uri4", ReviewId = reviewId2, Id = imageLinkId4 }
                ]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Name 3",
                PartnerId = "partner-3",
                TouristId = touristId2,
                Rating = 1,
                Comment = "Comment 3",
                CreatedAt = DateTime.MaxValue,
                ImageLinks =
                [
                    new() { IsTitle = true, Uri = "uri5", ReviewId = reviewId3, Id = imageLinkId5 },
                    new() { IsTitle = false, Uri = "uri6", ReviewId = reviewId3, Id = imageLinkId6 }
                ]
            }
        ];

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            await db.ObjectReviews.AddRangeAsync(testReviews);
            await db.SaveChangesAsync();
        }

        var client = _factory.CreateClient();
        client.BaseAddress = _baseAddress;

        // Act
        var response = await client.GetAsync(GetDtosIncludedByTouristUrl + Guid.NewGuid().ToString());
        var rawResult = await response.Content.ReadFromJsonAsync<IEnumerable<PartnerReviewDto>>();
        var result = rawResult?.ToList();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeEmpty();
    }
    #endregion
}