const BASE_URL = "http://localhost:3001/api";

export async function getAll() {
    const response = await fetch(`${BASE_URL}/customers`);
    return response.json();
}

import { Customer } from '../types';

export const GetAllCustomers = async (developerId: number): Promise<Customer[]> => {
    console.log("BBBBBBBBBBBBBBB", developerId);
    const response = await fetch(`${BASE_URL}/customers/getall`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ developerId }),
    });    
    if (!response.ok) {
        throw new Error('Failed to fetch customers');
    }
    const result = await response.json();
    return result.customers;
};

export const fetchCustomers = async (): Promise<Customer[]> => {
    return [];
};

export const createCustomer = async (customer: Customer): Promise<Customer> => {
    return customer;
};

export const updateCustomer = async (customer: Customer): Promise<void> => {    
    customer;
};

export const deleteCustomer = async (customerIds: number[]): Promise<void> => {    
    customerIds;
};
