namespace Kurs_ArendOff.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsPinnedToOrganizationData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrganizationDatas", "IsPinned", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrganizationDatas", "IsPinned");
        }
    }
}
