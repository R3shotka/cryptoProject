
import React, { ChangeEvent, JSX, useState, MouseEvent, SyntheticEvent } from 'react'

interface Props {
    onSearchSubmit: (e: SyntheticEvent) => void;
    search: string | undefined;
    handleSearchChange: (e: ChangeEvent<HTMLInputElement>) => void;
};

const Search: React.FC<Props> = ({ onSearchSubmit, search, handleSearchChange }: Props): JSX.Element => {

    return (

        <>
            <form onSubmit={onSearchSubmit}>
                <input value={search} onChange={handleSearchChange} className="w-full p-3 border-2 border-gray-300 rounded-lg focus:outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-200 transition-colors" />
            </form>
        </>
    )
}

export default Search