using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neudesic.Pulse.SharePoint.Online
{
    public static class Template
    {
        public const string EntityTypeMappings = 
            "<entityTypes>" +
              "<entityType code=\"item\" contentType=\"0x01\" />" +
              "<entityType code=\"document\" contentType=\"0x0101\" />" +
              "<entityType code=\"picture\" contentType=\"0x010102\" />" +
              "<entityType code=\"event\" contentType=\"0x0102\" />" +
              "<entityType code=\"issue\" contentType=\"0x0103\" />" +
              "<entityType code=\"announcement\" contentType=\"0x0104\" />" +
              "<entityType code=\"link\" contentType=\"0x0105\" />" +
              "<entityType code=\"contact\" contentType=\"0x0106\" />" +
              "<entityType code=\"task\" contentType=\"0x0108\" />" +
              "<entityType code=\"workflowtask\" contentType=\"0x010801\" />" +
              "<entityType code=\"folder\" contentType=\"0x0120\" />" +

              "<!--entityType code=\"site\" webTemplate=\"STS#1\" />" +
              "<entityType code=\"teamsite\" webTemplate=\"STS#0\" />" +
              "<entityType code=\"mysite\" webTemplate=\"SPSMSITE#0\" />" +

              "<entityType code=\"genericList\" listTemplate=\"100\" />" +
              "<entityType code=\"documentLibrary\" listTemplate=\"101\" />" +
              "<entityType code=\"announcementsList\" listTemplate=\"104\" />" +
              "<entityType code=\"eventsList\" listTemplate=\"106\" /-->" +
            "</entityTypes>";
    }
}
