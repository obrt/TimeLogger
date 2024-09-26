import React, { useState, useEffect } from "react";
import { ProjectTable } from "../components/ProjectTable";
import { getAllProjects, createProject, updateProject, deleteProject, finishProjects  } from "../api/projects";
import { GetAllCustomers } from "../api/customers";
import { GetAllDevelopers } from "../api/developers";
import { Project, Customer, Developer } from '../types';

export default function Projects() {
    const [projects, setProjects] = useState<Project[]>([]);
    const [customers, setCustomers] = useState<Customer[]>([]);
    const [developers, setDevelopers] = useState<Developer[]>([]);
    const [selectedProjects, setSelectedProjects] = useState<Project[]>([]);
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [showModal, setShowModal] = useState<boolean>(false);
    const [isUpdate, setIsUpdate] = useState<boolean>(false);
    const [formData, setFormData] = useState<Project>({
        id: 0, name: '', developerId: 0, developerName: '', customerId: 0, customerName: '', totalTimeLogged: 0, deadline: '', isFinished: false
    });
    const [showDeleteModal, setShowDeleteModal] = useState<boolean>(false);
    const developerId = Number(localStorage.getItem('developerId')) ?? 1;

    useEffect(() => {
        getAllProjects(developerId).then(setProjects).catch(error => console.error('Failed to fetch projects:', error));
        GetAllCustomers(developerId)
        .then(response => {
            setCustomers(response);
        })
        .catch(error => console.error('Failed to fetch customers:', error));
        GetAllDevelopers().then(setDevelopers).catch(error => console.error('Failed to fetch developers:', error));        
    }, []);

    const handleFinishProjects = async () => {
        const ids = selectedProjects.map(project => project.id);
        await finishProjects(ids);
        getAllProjects(developerId).then(setProjects);
        setSelectedProjects([]);
    };

    const handleSort = (column: string, order: 'asc' | 'desc' | 'default') => {
        const sorted = [...projects].sort((a, b) => {
            let comparison = 0;
            switch (column) {
                case 'name':
                    comparison = a.name.localeCompare(b.name);
                    break;
                case 'developerName':
                    comparison = a.developerName.localeCompare(b.developerName);
                    break;
                case 'totalTimeLogged':
                    comparison = a.totalTimeLogged - b.totalTimeLogged;
                    break;
                case 'deadline':
                    comparison = a.deadline.localeCompare(b.deadline);
                    break;
                default:
                    return 0;
            }
            return order === 'asc' ? comparison : -comparison;
        });

        setProjects(sorted);
    };

    const handleCreateOrUpdate = () => {
        if (selectedProjects.length === 1) {
            setIsUpdate(true);
            setFormData(selectedProjects[0]);
        } else {
            setIsUpdate(false);
            setFormData(prevFormData => ({
                ...prevFormData,
                id: 0,
                customerName: '', 
                developerName: '', 
                totalTimeLogged: 0,
                deadline: '',
                isFinished: false,
                developerId: developers.length > 0 ? developers[0].id : 0,
                customerId: projects.length > 0 ? projects[0].id : 0,
            }));
        }
        setShowModal(true);
    };

    const handleSave = async () => {
        if (isUpdate) {
            await updateProject(formData);
        } else {
            await createProject(formData);
        }
        setShowModal(false);
        getAllProjects(developerId).then(setProjects);
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLSelectElement | HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData(prevState => ({ ...prevState, [name]: value }));
    };

    const handleDelete = () => {
        if (selectedProjects.length > 0) {
            setShowDeleteModal(true);
        }
    };

    const confirmDelete = async () => {
        const idsToDelete = selectedProjects.map(project => project.id);
        await deleteProject(idsToDelete);
        setProjects(prevProjects => prevProjects.filter(project => !idsToDelete.includes(project.id)));
        setSelectedProjects([]);
        setShowDeleteModal(false);
    };

    const filteredProjects = projects.filter(project =>
        project.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        project.developerName.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <>
            <div className="justify-between items-center my-6 space-x-4 w-full">
                <div className="w-1/2 ml-5 mr-5">
                    <button                        
                        className={`bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded ${selectedProjects.length > 1 ? 'opacity-50 cursor-not-allowed' : ''}`}
                        onClick={handleCreateOrUpdate}
                        disabled={selectedProjects.length > 1}
                    >
                        {selectedProjects.length === 1 ? "Update Project" : "Create Project"}
                    </button>                

                    <button
                        className={`ml-2 bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 rounded ${selectedProjects.length > 0 ? '' : 'opacity-50 cursor-not-allowed'}`}
                        onClick={handleDelete}
                        disabled={selectedProjects.length === 0}
                    >
                        Delete
                    </button>

                    <button
                        className={`ml-2 bg-green-500 hover:bg-green-700 text-white font-bold py-2 px-4 rounded ${selectedProjects.length > 0 ? '' : 'opacity-50 cursor-not-allowed'}`}
                        onClick={handleFinishProjects}
                        disabled={selectedProjects.length === 0}
                    >
                        Finish them
                    </button>
                    <input 
                        className="border rounded-full py-2 px-4 mt-2"
                        type="search"
                        placeholder="Search Projects"
                        value={searchTerm}
                        onChange={e => setSearchTerm(e.target.value)}
                    />
                </div>
            </div>
                        
            <ProjectTable
                data={filteredProjects}
                selectedItems={selectedProjects}
                setSelectedItems={setSelectedProjects}
                onSort={handleSort}
            />

            {showDeleteModal && (
                <div className="modal fixed inset-0 flex justify-center items-center bg-gray-800 bg-opacity-50">
                    <div className="bg-white p-6 rounded shadow-lg w-1/3">
                        <h2 className="text-xl mb-4">Are you sure you want to delete the selected projects?</h2>
                        <div className="flex justify-end">
                            <button
                                type="button"
                                onClick={() => setShowDeleteModal(false)}
                                className="bg-gray-500 hover:bg-gray-700 text-white py-2 px-4 rounded mr-2"
                            >
                                Cancel
                            </button>
                            <button
                                type="button"
                                onClick={confirmDelete}
                                className="bg-red-500 hover:bg-red-700 text-white py-2 px-4 rounded"
                            >
                                Delete
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {showModal && (
                <div className="modal fixed inset-0 flex justify-center items-center bg-gray-800 bg-opacity-50">
                    <div className="bg-white p-6 rounded shadow-lg w-1/3">
                        <h2 className="text-xl mb-4">{isUpdate ? "Update Project" : "Create Project"}</h2>
                        <form>
                            <div className="mb-4">
                                <label className="block mb-2">Project Name:</label>
                                <input
                                    type="text"
                                    name="name"
                                    value={formData.name}
                                    onChange={handleInputChange}
                                    className="border py-2 px-4 w-full"
                                />
                            </div>
                            <div className="mb-4">
                                <label className="block mb-2">Developer:</label>
                                <select
                                    name="developerName"
                                    value={formData.developerName}
                                    onChange={handleInputChange}
                                    className="border py-2 px-4 w-full"
                                >
                                    <option value="" disabled>Select Developer</option>
                                    {developers && developers.length > 0 ? developers.map(developer => (
                                        <option key={developer.id} value={developer.firstName + " " + developer.lastName}>
                                            {developer.firstName + " " + developer.lastName}
                                        </option>
                                    )) : <option>No developers available</option>}
                                </select>
                            </div>

                            <div className="mb-4">
                                <label className="block mb-2">Customer:</label>
                                <select
                                    name="customerName"
                                    value={formData.customerName}
                                    onChange={handleInputChange}
                                    className="border py-2 px-4 w-full"
                                >
                                    <option value="" disabled>Select Customer</option>
                                    {customers && customers.length > 0 ? customers.map(customer => (
                                        <option key={customer.id} value={customer.name}>
                                            {customer.name}
                                        </option>
                                    )) : <option>No customers available</option>}
                                </select>
                            </div>
                            <div className="mb-4">
                                <label className="block mb-2">Deadline:</label>
                                <input
                                    type="date"
                                    name="deadline"
                                    value={formData.deadline}
                                    onChange={handleInputChange}
                                    className="border py-2 px-4 w-full"
                                />
                            </div>
                            <div className="mb-4">
                                <label className="block mb-2">Is Finished:</label>
                                <input
                                    type="checkbox"
                                    name="isFinished"
                                    checked={formData.isFinished}
                                    onChange={e => setFormData(prev => ({ ...prev, isFinished: e.target.checked }))}
                                    className="mr-2 leading-tight"
                                />
                                <span>Finished</span>
                            </div>
                            <div className="flex justify-end">
                                <button
                                    type="button"
                                    onClick={() => setShowModal(false)}
                                    className="bg-gray-500 hover:bg-gray-700 text-white py-2 px-4 rounded mr-2"
                                >
                                    Cancel
                                </button>
                                <button
                                    type="button"
                                    onClick={handleSave}
                                    className="bg-blue-500 hover:bg-blue-700 text-white py-2 px-4 rounded"
                                >
                                    {isUpdate ? "Update" : "Create"}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}
        </>
    );
}
