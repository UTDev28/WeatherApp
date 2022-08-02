using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using System.Net;
using Newtonsoft.Json.Linq; //Added with the Newtonsoft Nuget package manager
using Newtonsoft.Json;


namespace KRWeatherApp
{
    public partial class MainPage : ContentPage
    {
 

        //saves the API key as a constant
        const string API_KEY = "842edee0044f05ef8b3e4fb0850aba8b";
        public MainPage()
        {
            InitializeComponent();

            //Default city is Salt Lake City
            LoadWeatherDetails("Salt Lake City");
        }

        //Loads all weather details when the user enters a city
        public void LoadWeatherDetails(string city)
        {
            //webclient will take care of connecting to the API, sending requests, saving responses
            using(WebClient wc = new WebClient())
            {
                try
                {
                    //send a request to the API and save it to the JSON response to the string variable
                    string jsonData = wc.DownloadString($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={API_KEY}&units=imperial");

                    AllWeatherData weatherData = JsonConvert.DeserializeObject<AllWeatherData>(jsonData);

                    LblCity.Text = weatherData.name;

                    LblTemp.Text = $"{Math.Round(weatherData.main.temp, 0)}°F";

                    LblHighLow.Text = $"{Math.Round(weatherData.main.temp_max, 0)}°F\n" +
                        $"{Math.Round(weatherData.main.temp_min, 0)}°F";

                    LblWeatherCondition.Text = weatherData.weather[0].main;

                    LblWind.Text = $"{weatherData.wind.speed} mph";

                    //Show the pressure, humidity, and weather description

                    LblHumitity.Text = $"{weatherData.main.humidity}%";

                    LblPressure.Text = $"{weatherData.main.pressure} inHg";

                    LblDescription.Text = $"{weatherData.weather[0].description}";

                    //Display Weather icon
                    ImgWeather.Source = $"https://openweathermap.org/img/wn/{weatherData.weather[0].icon}@2x.png";

                    //Updates bg color according to temp
                    ChangeBackgroundColor(weatherData.main.temp, "imperial");

                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.Message, "Close");
                }
            }
        }

        //Shows weather details based on Zipcode OR City name (not both).
        private void BtnShowWeatherDetails_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(EntCity.Text))
            {
                LoadWeatherDetails(EntCity.Text);
            }
            else if (!string.IsNullOrWhiteSpace(EntZip.Text))
            {
                LoadWeatherlDetailByZip(EntZip.Text);
            }                 
            else
            {
                DisplayAlert("Error", "Please enter a valid city or Zipcode", "Close");
            }
        }


        //Allow the user to look up weather data using a zipcode.
        private void LoadWeatherlDetailByZip(string zipCode)
        {

            
            using (WebClient wc = new WebClient())
            {
                try
                {
                    string jsonData = wc.DownloadString($"https://api.openweathermap.org/data/2.5/weather?zip={zipCode}&appid={API_KEY}&units=imperial");

                    AllWeatherData weatherData = JsonConvert.DeserializeObject<AllWeatherData>(jsonData);

                    LblCity.Text = weatherData.name;

                    LblTemp.Text = $"{Math.Round(weatherData.main.temp, 0)}°F";

                    LblHighLow.Text = $"{Math.Round(weatherData.main.temp_max, 0)}°F\n" +
                        $"{Math.Round(weatherData.main.temp_min, 0)}°F";

                    LblWeatherCondition.Text = weatherData.weather[0].main;

                    LblWind.Text = $"{weatherData.wind.speed} mph";

                    //Show the pressure, humidity, and weather description

                    LblHumitity.Text = $"{weatherData.main.humidity}%";

                    LblPressure.Text = $"{weatherData.main.pressure} inHg";

                    LblDescription.Text = $"{weatherData.weather[0].description}";

                    //Updates bg color according to temp
                    ChangeBackgroundColor(weatherData.main.temp, "imperial");

                    //Display Weather icon
                    ImgWeather.Source = $"https://openweathermap.org/img/wn/{weatherData.weather[0].icon}@2x.png";



                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.Message, "Close");
                }
            }
        }

        //Add a conversion button(s) that will convert the temperature into Celsius and Fahrenheit
        private void ConvertUnits(string city)
        {
            using (WebClient wc = new WebClient())
            {
                try
                {
                    string jsonData = wc.DownloadString($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={API_KEY}&units=metric");

                    AllWeatherData weatherData = JsonConvert.DeserializeObject<AllWeatherData>(jsonData);

                    LblCity.Text = weatherData.name;

                    LblTemp.Text = $"{Math.Round(weatherData.main.temp, 0)}°C";

                    LblHighLow.Text = $"{Math.Round(weatherData.main.temp_max, 0)}°C\n" +
                        $"{Math.Round(weatherData.main.temp_min, 0)}°C";

                    LblWeatherCondition.Text = weatherData.weather[0].main;

                    LblWind.Text = $"{weatherData.wind.speed} mph";

                    //Show the pressure, humidity, and weather description

                    LblHumitity.Text = $"{weatherData.main.humidity}%";

                    LblPressure.Text = $"{weatherData.main.pressure} inHg";

                    LblDescription.Text = $"{weatherData.weather[0].description}";

                    //Updates bg color according to temp
                    ChangeBackgroundColor(weatherData.main.temp, "metric");

                    //Display Weather icon
                    ImgWeather.Source = $"https://openweathermap.org/img/wn/{weatherData.weather[0].icon}@2x.png";


                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.Message, "Close");
                }
            }
        }

        //When the user selects this button, it will convert the units with the ConvertUnits function
        private void BtnConversion_Clicked(object sender, EventArgs e)
        {
            ConvertUnits(LblCity.Text);
        }

        //The background will change depending on the temp in the selected area. This will work with 
        //whether the units are measured in metric OR fahrenheit.
        private void ChangeBackgroundColor(float temp, string units)
        {
            int hot;
            int cold;

            if (units == "imperial") 
            {
                hot = 80;
                cold = 40;
            }
            else
            {
                hot = 26;
                cold = 4;
            }

            if (temp > hot)
            {
                AppBackground.BackgroundColor = Color.Red;
            }
            else if (temp < cold)
            {
                AppBackground.BackgroundColor = Color.LightBlue;
            }
            else
            {
                AppBackground.BackgroundColor = Color.GhostWhite;
            }
        }

         private void BtnZipCodeSearch_Clicked(object sender, EventArgs e)
        {

        }

       
    }
}
