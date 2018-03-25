namespace EastMed.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniIdchanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("user", "UNI_ID", c => c.String(nullable: false, maxLength: 45, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("user", "UNI_ID", c => c.Int(nullable: false));
        }
    }
}
