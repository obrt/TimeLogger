import React, { useState, useEffect } from "react";
import { TimelogTable } from "../components/Table";
import { GetAllTimelogs, createTimelog, updateTimelog, deleteTimelog } from "../api/timelogs";
import { getAllProjects } from "../api/projects";
import { GetAllDevelopers } from "../api/developers";
import { Timelog, Developer, Project } from '../types';

export default function Timelogs() {
    const [timelogs, setTimelogs] = useState<Timelog[]>([]);
    const [selectedTimelogs, setSelectedTimelogs] = useState<Timelog[]>([]);
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [sortColumn, setSortColumn] = useState<string | null>(null);
    const [sortOrder, setSortOrder] = useState<'asc' | 'desc' | 'default'>('default');
    const [showModal, setShowModal] = useState<boolean>(false);
    const [isUpdate, setIsUpdate] = useState<boolean>(false);
    const [formData, setFormData] = useState<Timelog>({ id: 0, projectName: '', developerName: '', timeInMinutes: 0, developerId: 0, projectId: 0 });
    const [showDeleteModal, setShowDeleteModal] = useState<boolean>(false);
    const [developers, setDevelopers] = useState<Developer[]>([]);
    const [projects, setProjects] = useState<Project[]>([]);
    const developerId = Number(localStorage.getItem('developerId')) ?? 1;

    useEffect(() => {
        GetAllTimelogs(developerId).then(setTimelogs).catch(error => console.error('Failed to fetch timelogs:', error));
        GetAllDevelopers().then(setDevelopers).catch(error => console.error('Failed to fetch developers:', error));        
        getAllProjects(developerId).then(setProjects).catch(error => console.error('Failed to fetch projects:', error));
    }, []);

    const handleSort = (column: string) => {
        let newOrder: 'asc' | 'desc' | 'default' = 'asc';
        if (sortColumn === column) {
            if (sortOrder === 'asc') newOrder = 'desc';
            else if (sortOrder === 'desc') newOrder = 'default';
            else newOrder = 'asc';
        }
        setSortOrder(newOrder);
        setSortColumn(column);

        if (newOrder === 'default') {
            setTimelogs([...timelogs]);
            return;
        }

        const sorted = [...timelogs].sort((a, b) => {
            let comparison = 0;
            switch (column) {
                case 'projectName':
                    comparison = a.projectName.localeCompare(b.projectName);
                    break;
                case 'developerName':
                    comparison = a.developerName.localeCompare(b.developerName);
                    break;
                case 'id':
                    comparison = a.id - b.id;
                    break;
                case 'timeInMinutes':
                    comparison = a.timeInMinutes - b.timeInMinutes;
                    break;
                default:
                    return 0;
            }
            return newOrder === 'asc' ? comparison : -comparison;
        });

        setTimelogs(sorted);
    };

    const handleCreateOrUpdate = () => {
        if (selectedTimelogs.length === 1) {
            setIsUpdate(true);
            setFormData(selectedTimelogs[0]);
        } else {
            setIsUpdate(false);
            setFormData(prevFormData => ({
                ...prevFormData,
                id: 0,
                projectName: '', 
                developerName: '', 
                timeInMinutes: 0,
                developerId: developers.length > 0 ? developers[0].id : 0,
                projectId: projects.length > 0 ? projects[0].id : 0,
            }));
        }
        setShowModal(true);
    };

    const handleSave = async () => {        
        if (isUpdate) {
            await updateTimelog(formData);
        } else {
            await createTimelog(formData);
        }
        setShowModal(false);
        GetAllTimelogs(developerId).then(setTimelogs);
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLSelectElement | HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData(prevState => ({ ...prevState, [name]: value, [name]: name === 'timeInMinutes' ? Number(value) : value }));
    };

    const handleDelete = () => {
        if (selectedTimelogs.length > 0) {
            setShowDeleteModal(true);
        }
    };

    const confirmDelete = async () => {
        const idsToDelete = selectedTimelogs.map(timelog => timelog.id);
    
        if (idsToDelete.length === 0) {
            console.error('No timelogs selected for deletion.');
            return;
        }
        
        try {
            await deleteTimelog(idsToDelete);    
            setTimelogs(prevTimelogs => prevTimelogs.filter(timelog => !idsToDelete.includes(timelog.id)));
            setSelectedTimelogs([]);  
        } catch (error) {
            console.error('Failed to delete timelogs:', error);
        }
        setShowDeleteModal(false);  
    };

    const filteredTimelogs = timelogs.filter(timelog =>
        timelog.projectName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        timelog.developerName.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <>
            <div className="justify-between items-center my-6 space-x-4 w-full">
                <div className="w-1/2 ml-5 mr-5">
                    <button
                        className={`bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded ${selectedTimelogs.length > 1 ? 'opacity-50 cursor-not-allowed' : ''}`}
                        onClick={handleCreateOrUpdate}
                        disabled={selectedTimelogs.length > 1}
                    >
                        {selectedTimelogs.length === 1 ? "Update Timelog" : "Create Timelog"}
                    </button>
                
                    <button
                        className={`ml-2 bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 rounded ${selectedTimelogs.length > 0 ? '' : 'opacity-50 cursor-not-allowed'}`}
                        onClick={handleDelete}
                        disabled={selectedTimelogs.length === 0}
                    >
                        Delete
                    </button>
                    <input
                        className="border rounded-full py-2 px-4 mt-2"
                        type="search"
                        placeholder="Search Timelogs"
                        aria-label="Search Timelogs"
                        value={searchTerm}
                        onChange={e => setSearchTerm(e.target.value)}
                    />
                </div>
            </div>
                        
            <TimelogTable
                data={filteredTimelogs}
                selectedItems={selectedTimelogs}
                setSelectedItems={setSelectedTimelogs}
                onSort={handleSort}
            />

            {showDeleteModal && (
                <div className="modal fixed inset-0 flex justify-center items-center bg-gray-800 bg-opacity-50">
                    <div className="bg-white p-6 rounded shadow-lg w-1/3">
                        <h2 className="text-xl mb-4">Are you sure you want to delete the selected timelogs?</h2>
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
                        <h2 className="text-xl mb-4">{isUpdate ? "Update Timelog" : "Create Timelog"}</h2>
                        <form>
                            <div className="mb-4">
                                <label className="block mb-2">Project Name:</label>
                                <select
                                    name="projectName"
                                    value={formData.projectName}
                                    onChange={handleInputChange}
                                    className="border py-2 px-4 w-full"
                                >
                                    <option value="" disabled>Select Project</option>
                                    {projects.map(project => (
                                        <option key={project.id} value={project.name}>
                                            {project.name}
                                        </option>
                                    ))}
                                </select>
                            </div>
                            <div className="mb-4">
                                <label className="block mb-2">Developer Name:</label>
                                <select
                                    name="developerName"
                                    value={formData.developerName}
                                    onChange={handleInputChange}
                                    className="border py-2 px-4 w-full"
                                >
                                    <option value="" disabled>Select Developer</option>
                                    {developers.map(developer => (
                                        <option key={developer.id} value={developer.firstName + " " + developer.lastName}>
                                            {developer.firstName + " " + developer.lastName}
                                        </option>
                                    ))}
                                </select>
                            </div>
                            <div className="mb-4">
                                <label className="block mb-2">Time Logged (minutes):</label>
                                <input
                                    type="number"
                                    name="timeInMinutes"
                                    value={formData.timeInMinutes}
                                    onChange={handleInputChange}
                                    className="border py-2 px-4 w-full"
                                    required
                                />
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
