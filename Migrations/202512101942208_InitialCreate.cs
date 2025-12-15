namespace Kurs_ArendOff.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganizationDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrganizationName = c.String(),
                        INN = c.String(),
                        ContractNumber = c.String(),
                        ContractStartDate = c.DateTime(nullable: false),
                        ContractEndDate = c.DateTime(nullable: false),
                        PlaceIdentifier = c.String(),
                        RentalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPaid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Places",
                c => new
                    {
                        PlaceIdentifier = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Area = c.Double(nullable: false),
                        BaseRent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.PlaceIdentifier);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Places");
            DropTable("dbo.OrganizationDatas");
        }
    }
}
