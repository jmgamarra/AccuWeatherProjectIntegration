import React, { useState } from 'react';
import { Table, TableBody, TableCell, TableContainer, TableHead, TextField,TableRow,Button, Paper, CircularProgress, Typography, Container } from '@material-ui/core';
import WeatherApi from '../../WeatherApi';
import LocationDialog from './LocationDialog';
import './LocationList.css';
import axios from 'axios';
const LocationList = () => {
  const [locations, setLocations] = useState([]);
  const [loading, setLoading] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [openDialog, setOpenDialog] = useState(false);
  const [selectedForecasts, setSelectedForecast] = useState([]); 
  const getLocations = async () => {
    try {
      setLoading(true);
      //https://localhost:44377/WeatherForecast/locations?Location=per
      console.log(searchTerm);
      const response = await axios.get(`https://localhost:44377/WeatherForecast/locations?Location=${searchTerm}`);
      //const response = await WeatherApi.get('/WeatherForecast/locations?Location=per');
      setLocations(response.data);
      setLoading(false);
    } catch (error) {
      console.error('Error al obtener las ubicaciones:', error);
    }
  };
  
  const handleSearchChange = (event) => {
    setSearchTerm(event.target.value);
  };
  const handleSearchClick=(event)=>{
    getLocations();
  };
 const handleRowDoubleClick = async (location) => {
    try {
      const response = await axios.get(`https://localhost:44377/WeatherForecast/forecasts?SelectedKeyLocation=${location.key}`);
      setSelectedForecast(response.data.dailyForecasts);
      setOpenDialog(true); // Abre el popup con la información ya cargada
    } catch (error) {
      console.error('Error fetching forecast data:', error);
    }
  };
  const handleCloseDialog = () => {
    setOpenDialog(false);
    setSelectedForecast([]);
  };

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Locations List
      </Typography>
      <div style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
        <TextField
          label="Search City"
          variant="outlined"
          value={searchTerm}
          onChange={handleSearchChange}
        />
        <Button variant="contained" color="primary" onClick={handleSearchClick}>
          Search
        </Button>
      </div>
      {loading ? (
        <CircularProgress />
      ) : (
        <TableContainer component={Paper}>
          <Table aria-label="simple table">
            <TableHead>
              <TableRow>
                <TableCell className="hidden-column">Key</TableCell>
                <TableCell className='bold-header'>Name</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {locations.map((location, index) => (
                <TableRow key={index} onDoubleClick={() => handleRowDoubleClick(location)}>
                  <TableCell className="hidden-column">{location.key}</TableCell>
                  <TableCell>{location.localizedName}                </TableCell>
              </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      )}
         {selectedForecasts && (
        <LocationDialog
          open={openDialog}
          onClose={handleCloseDialog}
          forecasts={selectedForecasts}
        />
      )}
    </Container>
  );
};

export default LocationList;

