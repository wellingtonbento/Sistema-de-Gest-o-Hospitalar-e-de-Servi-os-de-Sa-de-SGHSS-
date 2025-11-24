using API_SGHSS.Context;
using API_SGHSS.DTOs.Mappings;
using API_SGHSS.Extensions;
using API_SGHSS.Repositories;
using API_SGHSS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? ConnectionDB = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SGHSSContext>(options =>
    options.UseMySql(ConnectionDB, ServerVersion.AutoDetect(ConnectionDB)));

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<DTOMappingProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
