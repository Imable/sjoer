using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.DataManagement
{
    class DataFactory : HelperClasses.Singleton<DataFactory>
    {
        public DataAdapter getDataAdapter(DataSources dataSource)
        {
            DataAdapter dataAdapter;

            switch (dataSource)
            {
                case DataSources.Postgres:
                    dataAdapter = new PostgresDataAdapter();
                    break;
                case DataSources.MarineTrafficAIS:
                    dataAdapter = new MarineTrafficAISDataAdapter();
                    break;
                case DataSources.Mock:
                    dataAdapter = new MockDataAdapter();
                    break;
                case DataSources.GPSInfo:
                    dataAdapter = new GPSInfoAdapter();
                    break;
                default:
                    throw new ArgumentException("No such data source", nameof(dataSource));
            }

            return dataAdapter;
        }

        public Connection getConnection(DataSources dataSource)
        {
            Connection connection;

            // TODO: Change returned connection to appropriate ones
            switch (dataSource)
            {
                case DataSources.Postgres:
                    connection = new GPSInfoConnection();
                    break;
                case DataSources.MarineTrafficAIS:
                    connection = new MarineTrafficAISConnection();
                    break;
                case DataSources.Mock:
                    connection = new GPSInfoConnection();
                    break;
                case DataSources.GPSInfo:
                    connection = new GPSInfoConnection();
                    break;
                default:
                    throw new ArgumentException("No such data source", nameof(dataSource));
            }

            return connection;
        }

        // This one might be unneccessary
        public DTO getDTO(DataSources dataSource)
        {
            // TODO: Assign proper DTO's
            DTO dto = new GPSInfoDTO();

            switch (dataSource)
            {
                case DataSources.GPSInfo:
                    dto = new GPSInfoDTO();
                    break;
                case DataSources.Postgres:
                    break;
                case DataSources.MarineTrafficAIS:
                    dto = new MarineTrafficAISDTO();
                    break;
                case DataSources.Mock:
                    break;
                default:
                    break;
            }

            return dto;
        }
    }
}
