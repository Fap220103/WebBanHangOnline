namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePosts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Posts", "CreatedBy", c => c.String());
            AddColumn("dbo.tb_Posts", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tb_Posts", "ModifiedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.tb_Posts", "ModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Posts", "ModifiedBy");
            DropColumn("dbo.tb_Posts", "ModifiedDate");
            DropColumn("dbo.tb_Posts", "CreatedDate");
            DropColumn("dbo.tb_Posts", "CreatedBy");
        }
    }
}
