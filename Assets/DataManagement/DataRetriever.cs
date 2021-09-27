using System.Collections;
using System.Collections.Generic;


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

        public DTO fetch()
        {
            return this.dataAdapter.convert(this.connection.get());
        }
    }
}