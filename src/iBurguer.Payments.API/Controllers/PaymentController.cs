using iBurguer.Payments.Core.UseCases.ConfirmPayment;
using iBurguer.Payments.Core.UseCases.GenerateQrCode;
using iBurguer.Payments.Core.UseCases.GetPaymentUseCase;
using iBurguer.Payments.Core.UseCases.RefusePaymentUseCase;
using Microsoft.AspNetCore.Mvc;

namespace iBurguer.Payments.API.Controllers;

/// <summary>
/// API controller for managing payments.
/// </summary>
[Route("api/payments")]
[ApiController]
public class PaymentController : ControllerBase
{
    /// <summary>
    /// Gets the payment of an order
    /// </summary>
    /// <remarks>Returns the payment generated for the order.</remarks>
    /// <param name="useCase">The use case responsible for retrieving the payment.</param>
    /// <param name="orderId">Id of the order that generated the payment.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <response code="200">Payment returned successfully.</response>
    /// <response code="422">Invalid request. Missing or invalid parameters.</response>
    /// <response code="500">Internal server error. Something went wrong on the server side.</response>
    /// <returns>Returns an HTTP response indicating the success of the operation along with the registered payment.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GenerateQrCodeResponse), 200)]
    public async Task<IActionResult> GetOrderPayment([FromServices] IGetPaymentUseCase useCase, [FromQuery] Guid orderId, CancellationToken cancellationToken = default)
    {
        var response = await useCase.GetPayment(orderId, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Register a new payment
    /// </summary>
    /// <remarks>Registers a new payment in the system.</remarks>
    /// <param name="useCase">The use case responsible for registering the payment.</param>
    /// <param name="request">The request containing information about the payment to be registered.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <response code="201">Payment created successfully.</response>
    /// <response code="422">Invalid request. Missing or invalid parameters.</response>
    /// <response code="500">Internal server error. Something went wrong on the server side.</response>
    /// <returns>Returns an HTTP response indicating the success of the operation along with the registered payment.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(GenerateQrCodeResponse), 201)]
    public async Task<ActionResult> GenerateQrCode([FromServices] IGenerateQrCodeUseCase useCase, GenerateQrCodeRequest request, CancellationToken cancellationToken = default)
    {
        var response = await useCase.GenerateQrCode(request, cancellationToken);

        return Created("Payment created successfully", response);
    }
    
    /// <summary>
    /// Confirm an existing payment
    /// </summary>
    /// <remarks>Marks an existing payment as confirmed in the system.</remarks>
    /// <param name="useCase">The use case responsible for confirming the payment.</param>
    /// <param name="id">The ID of the payment to be confirmed.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <response code="200">Payment confirmed successfully.</response>
    /// <response code="404">Payment not found.</response>
    /// <response code="500">Internal server error. Something went wrong on the server side.</response>
    /// <returns>Returns an HTTP response indicating the success of the operation along with the updated payment status.</returns>
    [HttpPatch("{id:guid}/confirmed")]
    [ProducesResponseType(typeof(PaymentConfirmedResponse), 200)]
    public async Task<ActionResult> ConfirmPayment([FromServices] IConfirmPaymentUseCase useCase, Guid id, CancellationToken cancellationToken = default)
    {
        var response = await useCase.ConfirmPayment(id, cancellationToken);

        return Ok(response);
    }
    
    /// <summary>
    /// Refuse an existing payment
    /// </summary>
    /// <remarks>Marks an existing payment as refused in the system.</remarks>
    /// <param name="useCase">The use case responsible for refusing the payment.</param>
    /// <param name="id">The ID of the payment to be refused.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <response code="200">Payment completed successfully.</response>
    /// <response code="404">Payment not found.</response>
    /// <response code="500">Internal server error. Something went wrong on the server side.</response>
    /// <returns>Returns an HTTP response indicating the success of the operation along with the updated payment status.</returns>
    [HttpPatch("{id:guid}/refused")]
    [ProducesResponseType(typeof(PaymentRefusedResponse), 200)]
    public async Task<ActionResult> RefusePayment([FromServices] IRefusePaymentUseCase useCase, Guid id, CancellationToken cancellationToken = default)
    {
        var response = await useCase.RefusePayment(id, cancellationToken);

        return Ok(response);
    }
}