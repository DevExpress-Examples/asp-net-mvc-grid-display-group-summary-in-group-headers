Namespace Controllers
    Public Class HomeController
        Inherits Controller

        Public Function Index() As ActionResult
            Return View()
        End Function
        Public Function GridViewPartial(ByVal resizing? As Boolean) As ActionResult
            ViewData("resizing") = resizing
            Return PartialView()
        End Function
    End Class
End Namespace