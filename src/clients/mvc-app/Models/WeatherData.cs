using System;
namespace mvc_app.Models;

public class WeatherData
{
	public DateTime Date { get; set; }
	public int TemperatureC { get; set; }
	public int TemperatureF { get; set; }
	public string Summary { get; set; }
}

