import React from 'react';
import { Table, TableBody, TableCell, TableContainer,TableHead,TableRow,Dialog,Paper, DialogTitle, DialogContent, DialogActions, Typography, Button } from '@material-ui/core';
const LocationDialog = ({ open, onClose, forecasts }) => {
    const formatDate = (dateString) => {
        const options = { day: '2-digit', month: 'numeric', year: 'numeric' };
        return new Date(dateString).toLocaleDateString('es-ES', options); // Usa el locale adecuado, 'es-ES' para español
    };
  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>Daily Forecast for the Next 5 Days</DialogTitle>
      <DialogContent>
      <TableContainer component={Paper}>
          <Table aria-label="simple table">
            <TableHead>
              <TableRow>
                <TableCell>Date</TableCell>
                <TableCell>Minimun</TableCell>
                <TableCell>Maximun</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {forecasts.map((forecast, index) => (
                <TableRow key={index} >
                 <TableCell>{formatDate(forecast.date)}</TableCell>
                 <TableCell>{forecast.temperature?.minimum?.value}°{forecast.temperature?.minimum?.unit}</TableCell>
                 <TableCell>{forecast.temperature?.maximum?.value}°{forecast.temperature?.maximum?.unit}</TableCell>
              </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} color="primary">
          Close
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default LocationDialog;
