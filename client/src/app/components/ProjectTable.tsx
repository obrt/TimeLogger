import React, { useState, useRef, useEffect } from 'react';
import { Project } from '../types';

interface ProjectTableProps {
    data: Project[];
    selectedItems: Project[];
    setSelectedItems: (items: Project[]) => void;
    onSort: (column: string, order: 'asc' | 'desc' | 'default') => void;
}

const ProjectTable: React.FC<ProjectTableProps> = ({ data, selectedItems, setSelectedItems, onSort }) => {
    const tableRef = useRef<HTMLTableElement>(null);
    const [sortColumn, setSortColumn] = useState<string | null>(null);
    const [sortOrder, setSortOrder] = useState<'asc' | 'desc' | 'default'>('default');

    const handleSort = (column: string) => {
        let newOrder: 'asc' | 'desc' | 'default' = 'asc';

        if (sortColumn === column) {
            if (sortOrder === 'asc') {
                newOrder = 'desc';
            } else if (sortOrder === 'desc') {
                newOrder = 'default';
            } else {
                newOrder = 'asc';
            }
        }

        setSortOrder(newOrder);
        setSortColumn(column);

        onSort(column, newOrder);
    };

    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            const target = event.target as Element;
            if (
                tableRef.current && 
                !tableRef.current.contains(target) && 
                !target.closest(".modal") &&  
                !target.closest("button")
            ) {
                setSelectedItems([]);
            }
        };
        document.addEventListener('click', handleClickOutside);

        return () => {
            document.removeEventListener('click', handleClickOutside);
        };
    }, [setSelectedItems]);

    const getSortIndicator = (column: string) => {
        if (sortColumn === column) {
            return sortOrder === 'asc' ? '↑' : sortOrder === 'desc' ? '↓' : '';
        }
        return '';
    };

    return (
        <div style={{ 
            height: '65vh',               
            overflowY: 'auto',            
            border: '1px solid #ccc', 
            padding: '5px', 
            borderRadius: '5px', 
            margin: '20px',               
        }}>
            <table ref={tableRef} className="w-full border-collapse">
                <thead>
                    <tr>
                        <th onClick={() => handleSort('name')} className="cursor-pointer p-4 bg-green-600 text-white">Project Name {getSortIndicator('name')}</th>
                        <th onClick={() => handleSort('developerName')} className="cursor-pointer p-4 bg-green-600 text-white">Developer {getSortIndicator('developerName')}</th>
                        <th onClick={() => handleSort('customerName')} className="cursor-pointer p-4 bg-green-600 text-white">Customer {getSortIndicator('customerName')}</th>
                        <th onClick={() => handleSort('totalTimeLogged')} className="cursor-pointer p-4 bg-green-600 text-white">Total Time {getSortIndicator('totalTimeLogged')}</th>
                        <th onClick={() => handleSort('deadline')} className="cursor-pointer p-4 bg-green-600 text-white">Deadline {getSortIndicator('deadline')}</th>
                        <th onClick={() => handleSort('isFinished')} className="cursor-pointer p-4 bg-green-600 text-white">Finished {getSortIndicator('isFinished')}</th>
                    </tr>
                </thead>
                <tbody>
                    {data.map((project) => (
                        <tr
                            key={project.id}
                            onClick={() => {
                                const isSelected = selectedItems.some(item => item.id === project.id);
                                setSelectedItems(isSelected
                                    ? selectedItems.filter(item => item.id !== project.id)
                                    : [...selectedItems, project]
                                );
                            }}
                            className={selectedItems.some(item => item.id === project.id) ? 'selected' : '' }
                        >
                            <td className="border p-4">{project.name}</td>
                            <td className="border p-4">{project.developerName}</td>
                            <td className="border p-4">{project.customerName}</td>
                            <td className="border p-4">{project.totalTimeLogged}</td>
                            <td className="border p-4">{project.deadline}</td>
                            <td className="border p-4">{project.isFinished ? 'Yes' : 'No'}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export { ProjectTable };
