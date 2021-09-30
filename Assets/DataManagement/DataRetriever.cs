using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assets.DataManagement
{
    public class DataRetriever
    {
        private DataSources dataSource;
        private DataAdapter dataAdapter;
        private Connection connection;

        public DataRetriever(DataSources dataSource)
        {
            this.dataSource = dataSource;
            dataAdapter = DataFactory.Instance.getDataAdapter(dataSource);
            connection = DataFactory.Instance.getConnection(dataSource);
        }

        public async Task<DTO> fetch(params string[] param)
        {
            return dataAdapter.convert(
                await connection.get(param)
                );
        }

        public bool isConnected()
        {
            return connection.connected;
        }
    }
}