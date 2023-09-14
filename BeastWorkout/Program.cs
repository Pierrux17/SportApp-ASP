using DAL.Repositories;
using DAL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddScoped<ICountryRepositoryDAL, CountryServiceDAL>();
builder.Services.AddScoped<IPersonRepositoryDAL, PersonServiceDAL>();
builder.Services.AddScoped<ITypePersonRepositoryDAL, TypePersonServiceDAL>();
builder.Services.AddScoped<ITypeProgramRepositoryDAL, TypeProgramServiceDAL>();
builder.Services.AddScoped<IProgramRepositoryDAL, ProgramServiceDAL>();
builder.Services.AddScoped<ISortExerciceRepositoryDAL, SortExerciceServiceDAL>();
builder.Services.AddScoped<ITypeExerciceRepositoryDAL, TypeExerciceServiceDAL>();
builder.Services.AddScoped<IExerciceRepositoryDAL, ExerciceServiceDAL>();
builder.Services.AddScoped<ITrainingRepositoryDAL, TrainingServiceDAL>();
builder.Services.AddScoped<IProfilRepositoryDAL, ProfilServiceDAL>();
builder.Services.AddScoped<ITrainingLogRepositoryDAL, TrainingLogServiceDAL>();
builder.Services.AddScoped<IExerciceLogRepositoryDAL, ExerciceLogServiceDAL>();
builder.Services.AddScoped<IPerformanceRepositoryDAL, PerformanceServiceDAL>();
builder.Services.AddScoped<IPersonProgramRepositoryDAL, PersonProgramServiceDAL>();  //Table d'association entre person et program
builder.Services.AddScoped<IProgramTrainingRepositoryDAL, ProgramTrainingServiceDAL>(); //Table d'association entre program et training
builder.Services.AddScoped<ITrainingExerciceRepositoryDAL, TrainingExerciceServiceDAL>(); //Table d'association entre training et exercice

builder.Services.AddScoped<IAuthRepositoryDAL, AuthServiceDAL>();

builder.Services.AddSession();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
