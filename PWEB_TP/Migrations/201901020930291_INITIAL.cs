namespace PWEB_TP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class INITIAL : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Candidaturas",
                c => new
                    {
                        CandidaturaId = c.Int(nullable: false, identity: true),
                        Ramo = c.Int(nullable: false),
                        Disciplinas = c.String(maxLength: 500),
                        Importancia = c.String(),
                        Orientador_FK = c.String(),
                        Estado = c.Int(nullable: false),
                        EstagioId = c.Int(nullable: false),
                        Observacoes = c.String(),
                        User_Id = c.String(maxLength: 128),
                        Orientador_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CandidaturaId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.Estagios", t => t.EstagioId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Orientador_Id)
                .Index(t => t.EstagioId)
                .Index(t => t.User_Id)
                .Index(t => t.Orientador_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Estagios",
                c => new
                    {
                        EstagioId = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Ramo = c.Int(nullable: false),
                        Enquandramento = c.String(nullable: false),
                        Objetivos = c.String(nullable: false),
                        CondicoesDeAcesso = c.String(nullable: false),
                        Local = c.String(nullable: false),
                        DataDeCriacao = c.DateTime(nullable: false),
                        Contacto = c.Int(nullable: false),
                        Estado = c.Int(nullable: false),
                        Observacoes = c.String(),
                        AvaliacaoED = c.Int(nullable: false),
                        AlunoId = c.String(maxLength: 128),
                        AvaliacaoA = c.Int(nullable: false),
                        DataDaDefesa = c.DateTime(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.EstagioId)
                .ForeignKey("dbo.AspNetUsers", t => t.AlunoId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.AlunoId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Disciplinas",
                c => new
                    {
                        DisciplinaId = c.Int(nullable: false, identity: true),
                        Ramo = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.DisciplinaId);
            
            CreateTable(
                "dbo.Mensagems",
                c => new
                    {
                        MensagemID = c.Int(nullable: false, identity: true),
                        Titulo = c.String(),
                        Conteudo = c.String(),
                        DataDeCriacao = c.DateTime(nullable: false),
                        Enviado_Id = c.String(maxLength: 128),
                        Recebido_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MensagemID)
                .ForeignKey("dbo.AspNetUsers", t => t.Enviado_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Recebido_Id)
                .Index(t => t.Enviado_Id)
                .Index(t => t.Recebido_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Mensagems", "Recebido_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Mensagems", "Enviado_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Candidaturas", "Orientador_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Candidaturas", "EstagioId", "dbo.Estagios");
            DropForeignKey("dbo.Estagios", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Estagios", "AlunoId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Candidaturas", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Mensagems", new[] { "Recebido_Id" });
            DropIndex("dbo.Mensagems", new[] { "Enviado_Id" });
            DropIndex("dbo.Estagios", new[] { "User_Id" });
            DropIndex("dbo.Estagios", new[] { "AlunoId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Candidaturas", new[] { "Orientador_Id" });
            DropIndex("dbo.Candidaturas", new[] { "User_Id" });
            DropIndex("dbo.Candidaturas", new[] { "EstagioId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Mensagems");
            DropTable("dbo.Disciplinas");
            DropTable("dbo.Estagios");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Candidaturas");
        }
    }
}
