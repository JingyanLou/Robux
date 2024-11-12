using Backend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 配置 SQLite 数据库连接
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db")); // 使用 SQLite 数据库，文件名为 app.db

// 注册控制器
builder.Services.AddControllers();

var app = builder.Build();

// 配置路由
app.MapControllers();

app.Run();