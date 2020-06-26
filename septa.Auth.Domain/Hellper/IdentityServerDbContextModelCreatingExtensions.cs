using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using septa.Auth.Domain.Constant;
using septa.Auth.Domain.Entities;

namespace septa.Auth.Domain.Hellper
{
    public static class IdentityServerDbContextModelCreatingExtensions
    {
        public static void ConfigureIdentityServer(this ModelBuilder builder)
        {
            builder.ConfigureClientContext();
            builder.ConfigurePersistedGrantContext();
            builder.ConfigureResourcesContext();
        }
        //public static void ConfigureIdentityServer(this ModelBuilder builder)
        //{

        //    builder.Entity<Client>(client =>
        //    {
        //        client.ToTable("Clients");

        //        client.Property(x => x.ClientId).HasMaxLength(ClientConsts.ClientIdMaxLength).IsRequired();
        //        client.Property(x => x.ProtocolType).HasMaxLength(ClientConsts.ProtocolTypeMaxLength).IsRequired();
        //        client.Property(x => x.ClientName).HasMaxLength(ClientConsts.ClientNameMaxLength);
        //        client.Property(x => x.ClientUri).HasMaxLength(ClientConsts.ClientUriMaxLength);
        //        client.Property(x => x.LogoUri).HasMaxLength(ClientConsts.LogoUriMaxLength);
        //        client.Property(x => x.Description).HasMaxLength(ClientConsts.DescriptionMaxLength);
        //        client.Property(x => x.FrontChannelLogoutUri).HasMaxLength(ClientConsts.FrontChannelLogoutUriMaxLength);
        //        client.Property(x => x.BackChannelLogoutUri).HasMaxLength(ClientConsts.BackChannelLogoutUriMaxLength);
        //        client.Property(x => x.ClientClaimsPrefix).HasMaxLength(ClientConsts.ClientClaimsPrefixMaxLength);
        //        client.Property(x => x.PairWiseSubjectSalt).HasMaxLength(ClientConsts.PairWiseSubjectSaltMaxLength);
        //        client.Property(x => x.UserCodeType).HasMaxLength(ClientConsts.UserCodeTypeMaxLength);

        //        client.HasMany(x => x.AllowedScopes).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
        //        client.HasMany(x => x.ClientSecrets).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
        //        client.HasMany(x => x.AllowedGrantTypes).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
        //        client.HasMany(x => x.AllowedCorsOrigins).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
        //        client.HasMany(x => x.RedirectUris).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
        //        client.HasMany(x => x.PostLogoutRedirectUris).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
        //        client.HasMany(x => x.IdentityProviderRestrictions).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
        //        client.HasMany(x => x.Claims).WithOne().HasForeignKey(x => x.ClientId).IsRequired();
        //        client.HasMany(x => x.Properties).WithOne().HasForeignKey(x => x.ClientId).IsRequired();

        //        client.HasIndex(x => x.ClientId);
        //    });

        //    builder.Entity<ClientGrantType>(grantType =>
        //    {
        //        grantType.ToTable("ClientGrantTypes");

        //        grantType.HasKey(x => new { x.ClientId, x.GrantType });

        //        grantType.Property(x => x.GrantType).HasMaxLength(ClientGrantTypeConsts.GrantTypeMaxLength).IsRequired();
        //    });

        //    builder.Entity<ClientRedirectUri>(redirectUri =>
        //    {
        //        redirectUri.ToTable("ClientRedirectUris");
        //        redirectUri.HasKey(x => new { x.ClientId, x.RedirectUri });
        //        redirectUri.Property(x => x.RedirectUri).HasMaxLength(ClientRedirectUriConsts.RedirectUriMaxLength).IsRequired();
        //    });

        //    builder.Entity<ClientPostLogoutRedirectUri>(postLogoutRedirectUri =>
        //    {
        //        postLogoutRedirectUri.ToTable("ClientPostLogoutRedirectUris");

        //        postLogoutRedirectUri.HasKey(x => new { x.ClientId, x.PostLogoutRedirectUri });
        //        postLogoutRedirectUri.Property(x => x.PostLogoutRedirectUri).HasMaxLength(ClientPostLogoutRedirectUriConsts.PostLogoutRedirectUriMaxLength).IsRequired();

        //    });

        //    builder.Entity<ClientScope>(scope =>
        //    {
        //        scope.ToTable("ClientScopes");

        //        scope.HasKey(x => new { x.ClientId, x.Scope });

        //        scope.Property(x => x.Scope).HasMaxLength(ClientScopeConsts.ScopeMaxLength).IsRequired();
        //    });

        //    builder.Entity<ClientSecret>(secret =>
        //    {
        //        secret.ToTable("ClientSecrets");

        //        secret.HasKey(x => new { x.ClientId, x.Type, x.Value });

        //        secret.Property(x => x.Type).HasMaxLength(SecretConsts.TypeMaxLength).IsRequired();

        //        secret.Property(x => x.Value).HasMaxLength(SecretConsts.ValueMaxLength).IsRequired();

        //        secret.Property(x => x.Description).HasMaxLength(SecretConsts.DescriptionMaxLength);
        //    });

        //    builder.Entity<ClientClaim>(claim =>
        //    {
        //        claim.ToTable("ClientClaims");

        //        claim.HasKey(x => new { x.ClientId, x.Type, x.Value });

        //        claim.Property(x => x.Type).HasMaxLength(ClientClaimConsts.TypeMaxLength).IsRequired();
        //        claim.Property(x => x.Value).HasMaxLength(ClientClaimConsts.ValueMaxLength).IsRequired();
        //    });

        //    builder.Entity<ClientIdPRestriction>(idPRestriction =>
        //    {
        //        idPRestriction.ToTable("ClientIdPRestrictions");

        //        idPRestriction.HasKey(x => new { x.ClientId, x.Provider });

        //        idPRestriction.Property(x => x.Provider).HasMaxLength(ClientIdPRestrictionConsts.ProviderMaxLength).IsRequired();
        //    });

        //    builder.Entity<ClientCorsOrigin>(corsOrigin =>
        //    {
        //        corsOrigin.ToTable("ClientCorsOrigins");

        //        corsOrigin.HasKey(x => new { x.ClientId, x.Origin });

        //        corsOrigin.Property(x => x.Origin).HasMaxLength(ClientCorsOriginConsts.OriginMaxLength).IsRequired();
        //    });

        //    builder.Entity<ClientProperty>(property =>
        //    {
        //        property.ToTable("ClientProperties");

        //        property.HasKey(x => new { x.ClientId, x.Key });

        //        property.Property(x => x.Key).HasMaxLength(ClientPropertyConsts.KeyMaxLength).IsRequired();
        //        property.Property(x => x.Value).HasMaxLength(ClientPropertyConsts.ValueMaxLength).IsRequired();
        //    });

        //    builder.Entity<PersistedGrant>(grant =>
        //    {
        //        grant.ToTable("PersistedGrants");
        //        grant.Property(x => x.Key).HasMaxLength(PersistedGrantConsts.KeyMaxLength).ValueGeneratedNever();
        //        grant.Property(x => x.Type).HasMaxLength(PersistedGrantConsts.TypeMaxLength).IsRequired();
        //        grant.Property(x => x.SubjectId).HasMaxLength(PersistedGrantConsts.SubjectIdMaxLength);
        //        grant.Property(x => x.ClientId).HasMaxLength(PersistedGrantConsts.ClientIdMaxLength).IsRequired();
        //        grant.Property(x => x.CreationTime).IsRequired();

        //        grant.Property(x => x.Data).HasMaxLength(PersistedGrantConsts.DataMaxLength).IsRequired();


        //        grant.HasKey(x => x.Key); //TODO: What about Id!!!

        //        grant.HasIndex(x => new { x.SubjectId, x.ClientId, x.Type });
        //        grant.HasIndex(x => x.Expiration);
        //    });

        //    builder.Entity<IdentityResource>(identityResource =>
        //    {
        //        identityResource.ToTable("IdentityResources");


        //        identityResource.Property(x => x.Name).HasMaxLength(IdentityResourceConsts.NameMaxLength).IsRequired();
        //        identityResource.Property(x => x.DisplayName).HasMaxLength(IdentityResourceConsts.DisplayNameMaxLength);
        //        identityResource.Property(x => x.Description).HasMaxLength(IdentityResourceConsts.DescriptionMaxLength);

        //        identityResource.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.IdentityResourceId).IsRequired();
        //    });

        //    builder.Entity<IdentityClaim>(claim =>
        //    {
        //        claim.ToTable("IdentityClaims");

        //        claim.HasKey(x => new { x.IdentityResourceId, x.Type });

        //        claim.Property(x => x.Type).HasMaxLength(UserClaimConsts.TypeMaxLength).IsRequired();
        //    });

        //    builder.Entity<ApiResource>(apiResource =>
        //    {
        //        apiResource.ToTable("ApiResources");

        //        apiResource.Property(x => x.Name).HasMaxLength(ApiResourceConsts.NameMaxLength).IsRequired();
        //        apiResource.Property(x => x.DisplayName).HasMaxLength(ApiResourceConsts.DisplayNameMaxLength);
        //        apiResource.Property(x => x.Description).HasMaxLength(ApiResourceConsts.DescriptionMaxLength);

        //        apiResource.HasMany(x => x.Secrets).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
        //        apiResource.HasMany(x => x.Scopes).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
        //        apiResource.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => x.ApiResourceId).IsRequired();
        //    });

        //    builder.Entity<ApiSecret>(apiSecret =>
        //    {
        //        apiSecret.ToTable("ApiSecrets");

        //        apiSecret.HasKey(x => new { x.ApiResourceId, x.Type, x.Value });

        //        apiSecret.Property(x => x.Type).HasMaxLength(SecretConsts.TypeMaxLength).IsRequired();
        //        apiSecret.Property(x => x.Description).HasMaxLength(SecretConsts.DescriptionMaxLength);
        //        apiSecret.Property(x => x.Value).HasMaxLength(SecretConsts.ValueMaxLength).IsRequired();

        //    });

        //    builder.Entity<ApiResourceClaim>(apiClaim =>
        //    {
        //        apiClaim.ToTable("ApiClaims");

        //        apiClaim.HasKey(x => new { x.ApiResourceId, x.Type });

        //        apiClaim.Property(x => x.Type).HasMaxLength(UserClaimConsts.TypeMaxLength).IsRequired();
        //    });

        //    builder.Entity<ApiScope>(apiScope =>
        //    {
        //        apiScope.ToTable("ApiScopes");

        //        apiScope.HasKey(x => new { x.ApiResourceId, x.Name });

        //        apiScope.Property(x => x.Name).HasMaxLength(ApiScopeConsts.NameMaxLength).IsRequired();
        //        apiScope.Property(x => x.DisplayName).HasMaxLength(ApiScopeConsts.DisplayNameMaxLength);
        //        apiScope.Property(x => x.Description).HasMaxLength(ApiScopeConsts.DescriptionMaxLength);

        //        apiScope.HasMany(x => x.UserClaims).WithOne().HasForeignKey(x => new { x.ApiResourceId, x.Name }).IsRequired();
        //    });

        //    builder.Entity<ApiScopeClaim>(apiScopeClaim =>
        //    {
        //        apiScopeClaim.ToTable("ApiScopeClaims");

        //        apiScopeClaim.HasKey(x => new { x.ApiResourceId, x.Name, x.Type });

        //        apiScopeClaim.Property(x => x.Type).HasMaxLength(UserClaimConsts.TypeMaxLength).IsRequired();
        //        apiScopeClaim.Property(x => x.Name).HasMaxLength(ApiScopeConsts.NameMaxLength).IsRequired();
        //    });

        //    builder.Entity<DeviceFlowCodes>(b =>
        //    {
        //        b.ToTable("DeviceFlowCodes");

        //        b.Property(x => x.DeviceCode).HasMaxLength(200).IsRequired();
        //        b.Property(x => x.UserCode).HasMaxLength(200).IsRequired();
        //        b.Property(x => x.SubjectId).HasMaxLength(200);
        //        b.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
        //        b.Property(x => x.Expiration).IsRequired();
        //        b.Property(x => x.Data).HasMaxLength(50000).IsRequired();

        //        b.HasIndex(x => new { x.UserCode }).IsUnique();
        //        b.HasIndex(x => x.DeviceCode).IsUnique();
        //        b.HasIndex(x => x.Expiration);
        //    });
        //}


        public static void ConfigureClientContext(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Client>(client =>
            {
                client.ToTable("Clients");
                client.HasKey(x => x.Id);

                client.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
                client.Property(x => x.ProtocolType).HasMaxLength(200).IsRequired();
                client.Property(x => x.ClientName).HasMaxLength(200);
                client.Property(x => x.ClientUri).HasMaxLength(2000);
                client.Property(x => x.LogoUri).HasMaxLength(2000);
                client.Property(x => x.Description).HasMaxLength(1000);
                client.Property(x => x.FrontChannelLogoutUri).HasMaxLength(2000);
                client.Property(x => x.BackChannelLogoutUri).HasMaxLength(2000);
                client.Property(x => x.ClientClaimsPrefix).HasMaxLength(200);
                client.Property(x => x.PairWiseSubjectSalt).HasMaxLength(200);
                client.Property(x => x.UserCodeType).HasMaxLength(100);
                client.Property(x => x.AllowedIdentityTokenSigningAlgorithms).HasMaxLength(100);

                client.HasIndex(x => x.ClientId).IsUnique();

                client.HasMany(x => x.AllowedGrantTypes).WithOne(x => x.Client).HasForeignKey(x => x.ClientId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.RedirectUris).WithOne(x => x.Client).HasForeignKey(x => x.ClientId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.PostLogoutRedirectUris).WithOne(x => x.Client).HasForeignKey(x => x.ClientId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.AllowedScopes).WithOne(x => x.Client).HasForeignKey(x => x.ClientId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.ClientSecrets).WithOne(x => x.Client).HasForeignKey(x => x.ClientId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.Claims).WithOne(x => x.Client).HasForeignKey(x => x.ClientId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.IdentityProviderRestrictions).WithOne(x => x.Client).HasForeignKey(x => x.ClientId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.AllowedCorsOrigins).WithOne(x => x.Client).HasForeignKey(x => x.ClientId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.Properties).WithOne(x => x.Client).HasForeignKey(x => x.ClientId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ClientGrantType>(grantType =>
            {
                grantType.ToTable("ClientGrantTypes");
                grantType.Property(x => x.GrantType).HasMaxLength(250).IsRequired();
            });

            modelBuilder.Entity<ClientRedirectUri>(redirectUri =>
            {
                redirectUri.ToTable("ClientRedirectUris");
                redirectUri.Property(x => x.RedirectUri).HasMaxLength(2000).IsRequired();
            });

            modelBuilder.Entity<ClientPostLogoutRedirectUri>(postLogoutRedirectUri =>
            {
                postLogoutRedirectUri.ToTable("ClientPostLogoutRedirectUris");
                postLogoutRedirectUri.Property(x => x.PostLogoutRedirectUri).HasMaxLength(2000).IsRequired();
            });

            modelBuilder.Entity<ClientScope>(scope =>
            {
                scope.ToTable("ClientScopes");
                scope.Property(x => x.Scope).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<ClientSecret>(secret =>
            {
                secret.ToTable("ClientSecrets");
                secret.Property(x => x.Value).HasMaxLength(4000).IsRequired();
                secret.Property(x => x.Type).HasMaxLength(250).IsRequired();
                secret.Property(x => x.Description).HasMaxLength(2000);
            });

            modelBuilder.Entity<ClientClaim>(claim =>
            {
                claim.ToTable("ClientClaims");
                claim.Property(x => x.Type).HasMaxLength(250).IsRequired();
                claim.Property(x => x.Value).HasMaxLength(250).IsRequired();
            });

            modelBuilder.Entity<ClientIdPRestriction>(idPRestriction =>
            {
                idPRestriction.ToTable("ClientIdPRestrictions");
                idPRestriction.Property(x => x.Provider).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<ClientCorsOrigin>(corsOrigin =>
            {
                corsOrigin.ToTable("ClientCorsOrigins");
                corsOrigin.Property(x => x.Origin).HasMaxLength(150).IsRequired();
            });

            modelBuilder.Entity<ClientProperty>(property =>
            {
                property.ToTable("ClientPropertys");
                property.Property(x => x.Key).HasMaxLength(250).IsRequired();
                property.Property(x => x.Value).HasMaxLength(2000).IsRequired();
            });
        }
        public static void ConfigureResourcesContext(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<IdentityResource>(identityResource =>
            {
                identityResource.ToTable("IdentityResources").HasKey(x => x.Id);

                identityResource.Property(x => x.Name).HasMaxLength(200).IsRequired();
                identityResource.Property(x => x.DisplayName).HasMaxLength(200);
                identityResource.Property(x => x.Description).HasMaxLength(1000);

                identityResource.HasIndex(x => x.Name).IsUnique();

                identityResource.HasMany(x => x.UserClaims).WithOne(x => x.IdentityResource).HasForeignKey(x => x.IdentityResourceId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                identityResource.HasMany(x => x.Properties).WithOne(x => x.IdentityResource).HasForeignKey(x => x.IdentityResourceId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<IdentityResourceClaim>(claim =>
            {
                claim.ToTable("IdentityResourceClaims").HasKey(x => x.Id);

                claim.Property(x => x.Type).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<IdentityResourceProperty>(property =>
            {
                property.ToTable("IdentityResourcePropertys");
                property.Property(x => x.Key).HasMaxLength(250).IsRequired();
                property.Property(x => x.Value).HasMaxLength(2000).IsRequired();
            });



            modelBuilder.Entity<ApiResource>(apiResource =>
            {
                apiResource.ToTable("ApiResources").HasKey(x => x.Id);

                apiResource.Property(x => x.Name).HasMaxLength(200).IsRequired();
                apiResource.Property(x => x.DisplayName).HasMaxLength(200);
                apiResource.Property(x => x.Description).HasMaxLength(1000);
                apiResource.Property(x => x.AllowedAccessTokenSigningAlgorithms).HasMaxLength(100);

                apiResource.HasIndex(x => x.Name).IsUnique();

                apiResource.HasMany(x => x.Secrets).WithOne(x => x.ApiResource).HasForeignKey(x => x.ApiResourceId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                apiResource.HasMany(x => x.Scopes).WithOne(x => x.ApiResource).HasForeignKey(x => x.ApiResourceId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                apiResource.HasMany(x => x.UserClaims).WithOne(x => x.ApiResource).HasForeignKey(x => x.ApiResourceId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                apiResource.HasMany(x => x.Properties).WithOne(x => x.ApiResource).HasForeignKey(x => x.ApiResourceId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ApiResourceSecret>(apiSecret =>
            {
                apiSecret.ToTable("ApiResourceSecrets").HasKey(x => x.Id);

                apiSecret.Property(x => x.Description).HasMaxLength(1000);
                apiSecret.Property(x => x.Value).HasMaxLength(4000).IsRequired();
                apiSecret.Property(x => x.Type).HasMaxLength(250).IsRequired();
            });

            modelBuilder.Entity<ApiResourceClaim>(apiClaim =>
            {
                apiClaim.ToTable("ApiResourceClaims").HasKey(x => x.Id);

                apiClaim.Property(x => x.Type).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<ApiResourceScope>((System.Action<EntityTypeBuilder<ApiResourceScope>>)(apiScope =>
            {
                apiScope.ToTable("ApiResourceScopes").HasKey(x => x.Id);

                apiScope.Property(x => x.Scope).HasMaxLength(200).IsRequired();
            }));

            modelBuilder.Entity<ApiResourceProperty>(property =>
            {
                property.ToTable("ApiResourceProperties");
                property.Property(x => x.Key).HasMaxLength(250).IsRequired();
                property.Property(x => x.Value).HasMaxLength(2000).IsRequired();
            });


            modelBuilder.Entity<ApiScope>(scope =>
            {
                scope.ToTable("ApiScopes").HasKey(x => x.Id);

                scope.Property(x => x.Name).HasMaxLength(200).IsRequired();
                scope.Property(x => x.DisplayName).HasMaxLength(200);
                scope.Property(x => x.Description).HasMaxLength(1000);

                scope.HasIndex(x => x.Name).IsUnique();

                scope.HasMany(x => x.UserClaims).WithOne(x => x.Scope).HasForeignKey(x => x.ScopeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<ApiScopeClaim>(scopeClaim =>
            {
                scopeClaim.ToTable("ApiScopeClaims").HasKey(x => x.Id);

                scopeClaim.Property(x => x.Type).HasMaxLength(200).IsRequired();
            });
            modelBuilder.Entity<ApiScopeProperty>(property =>
            {
                property.ToTable("ApiScopePropertys").HasKey(x => x.Id);
                property.Property(x => x.Key).HasMaxLength(250).IsRequired();
                property.Property(x => x.Value).HasMaxLength(2000).IsRequired();
            });


        }
        public static void ConfigurePersistedGrantContext(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PersistedGrant>(grant =>
            {
                grant.ToTable("PersistedGrants");

                grant.Property(x => x.Key).HasMaxLength(200).ValueGeneratedNever();
                grant.Property(x => x.Type).HasMaxLength(50).IsRequired();
                grant.Property(x => x.SubjectId).HasMaxLength(200);
                grant.Property(x => x.SessionId).HasMaxLength(100);
                grant.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
                grant.Property(x => x.Description).HasMaxLength(200);
                grant.Property(x => x.CreationTime).IsRequired();
                // 50000 chosen to be explicit to allow enough size to avoid truncation, yet stay beneath the MySql row size limit of ~65K
                // apparently anything over 4K converts to nvarchar(max) on SqlServer
                grant.Property(x => x.Data).HasMaxLength(50000).IsRequired();

                grant.HasKey(x => x.Key);

                grant.HasIndex(x => new { x.SubjectId, x.ClientId, x.Type });
                grant.HasIndex(x => new { x.SubjectId, x.SessionId, x.Type });
                grant.HasIndex(x => x.Expiration);
            });

            modelBuilder.Entity<DeviceFlowCodes>(codes =>
            {
                codes.ToTable("DeviceFlowCodes");

                codes.Property(x => x.DeviceCode).HasMaxLength(200).IsRequired();
                codes.Property(x => x.UserCode).HasMaxLength(200).IsRequired();
                codes.Property(x => x.SubjectId).HasMaxLength(200);
                codes.Property(x => x.SessionId).HasMaxLength(100);
                codes.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
                codes.Property(x => x.Description).HasMaxLength(200);
                codes.Property(x => x.CreationTime).IsRequired();
                codes.Property(x => x.Expiration).IsRequired();
                // 50000 chosen to be explicit to allow enough size to avoid truncation, yet stay beneath the MySql row size limit of ~65K
                // apparently anything over 4K converts to nvarchar(max) on SqlServer
                codes.Property(x => x.Data).HasMaxLength(50000).IsRequired();

                codes.HasKey(x => new { x.UserCode });

                codes.HasIndex(x => x.DeviceCode).IsUnique();
                codes.HasIndex(x => x.Expiration);
            });
        }




    }

}
