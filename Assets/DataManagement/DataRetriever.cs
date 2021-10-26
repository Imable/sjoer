using Assets.Positional;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.DataManagement
{
    public class DataRetriever
    {
        private DataAdapter dataAdapter;
        private Connection connection;
        private ParameterExtractor parameterExtractor;

        public DataRetriever(DataSources dataSource, WorldAligner aligner)
        {
            // Assign aligner to the DataFactory if DataFactory.Instance.aligner is null
            DataFactory.Instance.aligner ??= aligner;
            dataAdapter = DataFactory.Instance.getDataAdapter(dataSource);
            connection = DataFactory.Instance.getConnection(dataSource);
            parameterExtractor = DataFactory.Instance.getParameterExtractor(dataSource);
        }

        public async Task<DTO> fetch()
        {
            //Debug.Log(await connection.get(parameterExtractor.get()));
            return dataAdapter.convert(
                await connection.get(parameterExtractor.get())
                );
        }

        public bool isConnected()
        {
            return connection.connected;
        }
    }
}