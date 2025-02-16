import React from 'react';

const AppointmentCard = ({ appointment }) => {
    const formatDate = (date) => {
        return new Date(date).toLocaleString();
    };

    return (
        <div className="border rounded-lg p-4 shadow-sm hover:shadow-md transition-shadow">
            <h3 className="font-semibold text-lg mb-2">{appointment.serviceName}</h3>
            <div className="space-y-2 text-sm">
                <p><span className="font-medium">Customer:</span> {appointment.customerName}</p>
                <p><span className="font-medium">Phone:</span> {appointment.customerPhone}</p>
                <p><span className="font-medium">Start:</span> {formatDate(appointment.bookedFrom)}</p>
                <p><span className="font-medium">End:</span> {formatDate(appointment.bookedTill)}</p>
                <p><span className="font-medium">Service ID:</span> {appointment.serviceId}</p>
            </div>
        </div>
    );
};

export default AppointmentCard;