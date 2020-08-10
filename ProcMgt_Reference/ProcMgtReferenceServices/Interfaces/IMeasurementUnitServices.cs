using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProcMgt_Reference_Core.Models;
using ProcMgt_Reference_Core.Resources;
using ProcMgt_Reference_Services.Communication;

namespace ProcMgt_Reference_Services.Interfaces
{
   public interface IMeasurementUnitServices
    {
        Task<IEnumerable<MeasurementUnits>> GetAllAsync();
        Task<GenericSaveResponse<MeasurementUnits>> SaveMeasurementUnitsAsync(MeasurementUnits measurementunits);
        Task<GenericSaveResponse<MeasurementUnits>> UpdateMeasurementUnitsAsync(string id, MeasurementUnits measurementunits);
        Task<GenericSaveResponse<MeasurementUnits>> DeleteMeasurementUnitsAsync(string id, MeasurementUnits measurementunits);

        Task<DataGridTable> GetMeasurementUnitGridAsync();
    }
}
