import React from 'react';
import AddUserAssetsBalance from '../UserAssetsBalance/AddUserAssetsBalance/AddUserAssetsBalance';
import { Link } from 'react-router-dom';

interface CryptoSearchResult {
    symbol: string;
    name: string;
    externalId: string;
    logoUrl?: string;
}

interface Props {
    id: string;
    searchResult: CryptoSearchResult;
    onUserAssetsBalanceCreate: (e: React.SyntheticEvent) => void;
}

const Card: React.FC<Props> = ({ id, searchResult, onUserAssetsBalanceCreate }: Props) => {
    return (
        <div
            className="flex flex-col items-center justify-between w-full p-6 mb-4 bg-slate-100 rounded-lg md:flex-row shadow-sm gap-4"
            id={id}
        >
            {searchResult.logoUrl && (
                <img
                    alt={`${searchResult.name} logo`}
                    src={searchResult.logoUrl}
                    className="w-10 h-10 rounded-full"
                />
            )}

            <Link to={`/crypto/${searchResult.externalId}`} className="text-lg font-bold text-center text-gray-900 md:text-left w-full md:w-auto">
                {searchResult.name} ({searchResult.symbol.toUpperCase()})
            </Link>

            <p className="font-medium text-gray-700">
                Cryptocurrency
            </p>

            <AddUserAssetsBalance
                onUserAssetsBalanceCreate={onUserAssetsBalanceCreate}
                symbol={searchResult.symbol}
            />
        </div>
    )
}

export default Card;