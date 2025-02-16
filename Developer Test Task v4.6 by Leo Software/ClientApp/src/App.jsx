import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Login from './components/Auth/Login';
import CalendarView from './components/Calendar/CalendarView';

const App = () => {
    const isAuthenticated = () => !!sessionStorage.getItem('booksy_token');

    return (
        <Router>
            <Routes>
                <Route
                    path="/login"
                    element={isAuthenticated() ? <Navigate to="/calendar" /> : <Login />}
                />
                <Route
                    path="/calendar"
                    element={isAuthenticated() ? <CalendarView /> : <Navigate to="/login" />}
                />
                <Route
                    path="/"
                    element={<Navigate to="/login" />}
                />
            </Routes>
        </Router>
    );
};

export default App;