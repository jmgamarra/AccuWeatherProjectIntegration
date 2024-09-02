import React, { useState,useEffect } from 'react';
import { Table, TableBody, TableCell, TableContainer, TableHead, TextField,TableRow,TablePagination,Button, Paper, CircularProgress, Typography, Container } from '@material-ui/core';
import './LocationList.css';
import ForecastDialog from './ForecastDialog';
import { getLocations, getForecasts } from '../../services/WeatherServices';  
import Location from '../../models/Location';
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

 
    const getData = async () => {
      setLoading(true);
      try {
        const locationData = await getLocations(searchTerm, currentPage, rowsPerPage);
        const loadEntityLocations = locationData.data.map(loc => new Location(loc.key, loc.localizedName, []));
        setLocations(loadEntityLocations);
        setTotalLocations(locationData.totalCount);
        setLoading(false);
      } catch (error) {
        console.error('Error retrieving data:', error);
      } finally {
        setLoading(false);
      }
    };
    useEffect(() => {
      if(searchTerm){
        getData();
      }
    }, [searchTerm,currentPage, rowsPerPage]);
   
  

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
    setCurrentPage(1);
    getData();
  };
 const handleRowDoubleClick = async (location) => {
    try {
      const forecastData = await getForecasts(location.key);
      setSelectedForecast(forecastData.dailyForecasts);
      setOpenDialog(true);
    } catch (error) {
      console.error('Error retrieving forecast detail:', error);
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
        <ForecastDialog
          open={openDialog}
          onClose={handleCloseDialog}
          forecasts={selectedForecasts}
        />
      )}
    </Container>
  );
};

export default LocationList;

