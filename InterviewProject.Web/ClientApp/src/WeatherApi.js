import axios from 'axios';
const url=process.env.REACT_APP_API_BASE_URL;
console.log(url);
const WeatherApi = axios.create({
  baseURL: process.env.REACT_APP_API_BASE_URL,
  timeout: 1000,
  headers: { 'Content-Type': 'application/json' }
});
export default WeatherApi;