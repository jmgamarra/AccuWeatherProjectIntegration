import axios from 'axios';
export const getLocations = async (location, page = 1, pageSize = 10) => {
    try {
      const response = await axios.get(`${process.env.REACT_APP_API_BASE_URL}/WeatherForecast/locations`, {
        params: {
          Location: location,
          page: page,
          size: pageSize
        }
      });
      return response.data;
    } catch (error) {
      console.error('Error retrieving locations:', error);
      throw error; 
    }
  };

  export const getForecasts = async (locationKey) => {
    try {
      const response = await axios.get(`${process.env.REACT_APP_API_BASE_URL}/WeatherForecast/forecasts`, {
        params: {
          SelectedKeyLocation: locationKey
        }
      });
      return response.data;
    } catch (error) {
      console.error('Error retrieving forecasts detail:', error);
      throw error; 
    }
  };