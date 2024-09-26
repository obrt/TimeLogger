import React, { useState, useEffect, useRef } from 'react';
import { Timelog } from '../types';

interface TimelogTableProps {
    data: Timelog[];
    selectedItems: Timelog[];
    setSelectedItems: (items: Timelog[]) => void;
    onSort: (column: string, order: 'asc' | 'desc' | 'default') => void;
}

const TimelogTable: React.FC<TimelogTableProps> = ({ data, selectedItems, setSelectedItems, onSort }) => {
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
            <table ref={tableRef} style={{ width: '100%', borderCollapse: 'collapse' }}>
                <thead>
                    <tr>
                        <th onClick={() => handleSort('id')}>Id {getSortIndicator('id')}</th>
                        <th onClick={() => handleSort('developerName')}>Developer {getSortIndicator('developerName')}</th>
                        <th onClick={() => handleSort('projectName')}>Project {getSortIndicator('projectName')}</th>
                        <th onClick={() => handleSort('timeInMinutes')}>Time Logged {getSortIndicator('timeInMinutes')}</th>
                    </tr>
                </thead>
                <tbody>
                    {data.map(timelog => (
                        <tr
                            key={timelog.id}
                            onClick={() => {
                                const isSelected = selectedItems.some(item => item.id === timelog.id);
                                setSelectedItems(isSelected
                                    ? selectedItems.filter(item => item.id !== timelog.id)
                                    : [...selectedItems, timelog]
                                );
                            }}
                            className={selectedItems.some(item => item.id === timelog.id) ? 'selected' : ''}
                        >
                            <td>{timelog.id}</td>
                            <td>{timelog.developerName}</td>
                            <td>{timelog.projectName}</td>
                            <td>{timelog.timeInMinutes}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export { TimelogTable };
