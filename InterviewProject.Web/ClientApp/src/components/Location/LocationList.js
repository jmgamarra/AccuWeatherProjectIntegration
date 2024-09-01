import React, { useState,useEffect } from 'react';
import { Table, TableBody, TableCell, TableContainer, TableHead, TextField,TableRow,TablePagination,Button, Paper, CircularProgress, Typography, Container } from '@material-ui/core';
import WeatherApi from '../../WeatherApi';
import LocationDialog from './LocationDialog';
import './LocationList.css';
import axios from 'axios';
const LocationList = () => {
  const defaultPageSize = parseInt(process.env.REACT_APP_PAGE_SIZE,10) || 10;
  const [locations, setLocations] = useState([]);
  const [loading, setLoading] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [openDialog, setOpenDialog] = useState(false);
  const [selectedForecasts, setSelectedForecast] = useState([]); 
  const [currentPage, setCurrentPage] = useState(1);
  const [rowsPerPage, setRowsPerPage] = useState(defaultPageSize);
  const [totalLocations, setTotalLocations] = useState(0);

  useEffect(() => {
    getLocations(currentPage);
  }, [currentPage, rowsPerPage]);
  const getLocations = async (page) => {
    try {
      setLoading(true);
      //https://localhost:44377/WeatherForecast/locations?Location=per
      const response = await axios.get('https://localhost:44377/WeatherForecast/locations', {
        params: {
          Location: searchTerm,
          page: page,
          size: rowsPerPage
        }
      });
      //const response = await WeatherApi.get('/WeatherForecast/locations?Location=per');
      console.log('API Response:', response.data);
      setLocations(response.data.data);
      setTotalLocations(response.data.totalCount);
      setLoading(false);
    } catch (error) {
      console.error('Error al obtener las ubicaciones:', error);
    } finally {
      setLoading(false);
    }
  };
  const handlePageChange = (event, newPage) => {
    setCurrentPage(newPage + 1);
  };
  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    //setPage(0);
    setCurrentPage(1);
  };
  const handleSearchChange = (event) => {
    setSearchTerm(event.target.value);
  };
  const handleSearchClick=(event)=>{
    getLocations(1);
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
                  <TableCell>{location.localizedName}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
          <TablePagination
            rowsPerPageOptions={[]}
            component="div"
            count={totalLocations}
            rowsPerPage={rowsPerPage}
            page={currentPage-1}
            onPageChange={handlePageChange}
            onRowsPerPageChange={handleChangeRowsPerPage}
          />
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

