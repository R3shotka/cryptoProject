import React, { useEffect, useState } from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import axios from 'axios';

interface PricePoint {
    timestamp: number;
    price: number;
}

const BitcoinChart: React.FC = () => {
    const [data, setData] = useState<any[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string>('');

    useEffect(() => {
        const fetchData = async () => {
            try {
                setLoading(true);
                const response = await axios.get<PricePoint[]>('http://localhost:5144/api/crypto/chart/bitcoin?days=7');

                const chartData = response.data.map((point) => ({
                    date: new Date(point.timestamp).toLocaleDateString('en-US', { month: 'short', day: 'numeric' }),
                    price: point.price
                }));

                setData(chartData);
            } catch (err) {
                setError('Failed to load chart data');
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    if (loading) {
        return (
            <div className="w-full bg-white rounded-3xl shadow-2xl p-6 overflow-hidden">
                <div className="h-[400px] flex items-center justify-center text-gray-500">Loading chart...</div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="w-full bg-white rounded-3xl shadow-2xl p-6 overflow-hidden">
                <div className="h-[400px] flex items-center justify-center text-red-500">{error}</div>
            </div>
        );
    }

    return (
        <div className="w-full bg-white rounded-3xl shadow-2xl p-6 overflow-hidden">
            <h3 className="text-2xl font-bold text-gray-900 mb-4">Bitcoin Price (7 days)</h3>
            <div className="w-full h-[400px]">
                <ResponsiveContainer width="100%" height="100%">
                    <LineChart data={data}>
                        <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
                        <XAxis
                            dataKey="date"
                            stroke="#6b7280"
                            style={{ fontSize: '12px' }}
                        />
                        <YAxis
                            stroke="#6b7280"
                            style={{ fontSize: '12px' }}
                            tickFormatter={(value) => `$${value.toLocaleString()}`}
                        />
                        <Tooltip
                            formatter={(value: any) => [`$${Number(value).toLocaleString()}`, 'Price']}
                            contentStyle={{
                                backgroundColor: 'white',
                                border: '1px solid #e5e7eb',
                                borderRadius: '8px'
                            }}
                        />
                        <Line
                            type="monotone"
                            dataKey="price"
                            stroke="#3b82f6"
                            strokeWidth={2}
                            dot={false}
                        />
                    </LineChart>
                </ResponsiveContainer>
            </div>
        </div>
    );
};

export default BitcoinChart;
