import React, { JSX } from 'react'
import Card from '../Card/Card'

interface CryptoSearchResult {
    symbol: string;
    name: string;
    externalId: string;
    logoUrl?: string;
}

interface Props {
    searchResults: CryptoSearchResult[];
    onUserAssetsBalanceCreate: (e: React.SyntheticEvent) => void;
}

const CardList: React.FC<Props> = ({ searchResults, onUserAssetsBalanceCreate }: Props): JSX.Element => {
    return (
        <div>
            {searchResults.length > 0 ? (
                searchResults.map((crypto) => (
                    <Card key={crypto.externalId} id={crypto.symbol} searchResult={crypto} onUserAssetsBalanceCreate={onUserAssetsBalanceCreate} />
                ))
            ) : (
                <p className="mb-3 mt-3 text-xl font-semibold text-center md:text-xl">
                    No results!
                </p>
            )}
        </div>
    );
}

export default CardList