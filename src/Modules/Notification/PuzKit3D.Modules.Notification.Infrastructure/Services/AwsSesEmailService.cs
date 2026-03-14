using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Options;
using PuzKit3D.Modules.Notification.Application.Services;
using PuzKit3D.Modules.Notification.Domain.Emails;
using PuzKit3D.Modules.Notification.Infrastructure.DependencyInjection.Options;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Notification.Infrastructure.Services;

public sealed class AwsSesEmailService : IEmailService
{
    private readonly IAmazonSimpleEmailService _ses;
    private readonly EmailSettings _emailSettings;

    public AwsSesEmailService(IAmazonSimpleEmailService ses, IOptions<EmailSettings> options)
    {
        _ses = ses;
        _emailSettings = options.Value;
    }

    public async Task<Result> SendAsync(string toEmail, string subject, string body)
    {
        var request = new SendEmailRequest
        {
            Source = _emailSettings.SenderEmail,
            Destination = new Destination
            {
                ToAddresses = [toEmail]
            },
            Message = new Message
            {
                Subject = new Content(subject),
                Body = new Body
                {
                    Html = new Content(body)
                }
            }
        };

        var response = await _ses.SendEmailAsync(request);

        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            return Result.Failure(EmailError.FailedSendEmail());

        return Result.Success();
    }

    public async Task InitializeEmailTemplate()
    {
        var orderCreatedSuccessEmailTemplate = new CreateTemplateRequest
        {
            Template = new Template
            {
                TemplateName = EmailTemplate.OrderCreatedSuccessEmailTemplate,
                SubjectPart = "PuzKit3D - Your order has been created successfully !",
                TextPart = """
                <!DOCTYPE html>
                <html lang="vi" xmlns="http://www.w3.org/1999/xhtml">
                  <head>
                    <meta charset="UTF-8" />
                    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
                    <title>Đặt hàng thành công – PuzKit3D</title>
                    <style>
                      /* =============================================
                         Email Reset & Base
                         ============================================= */
                      * {
                        margin: 0;
                        padding: 0;
                        box-sizing: border-box;
                      }
                      body,
                      table,
                      td,
                      a {
                        -webkit-text-size-adjust: 100%;
                        -ms-text-size-adjust: 100%;
                      }
                      table {
                        border-collapse: collapse !important;
                        mso-table-lspace: 0pt;
                        mso-table-rspace: 0pt;
                      }
                      img {
                        border: 0;
                        height: auto;
                        line-height: 100%;
                        outline: none;
                        text-decoration: none;
                        -ms-interpolation-mode: bicubic;
                      }
                      body {
                        background-color: #eef1f6;
                        font-family: 'Segoe UI', Arial, Helvetica, sans-serif;
                        font-size: 15px;
                        color: #1e2a45;
                      }

                      /* =============================================
                         Wrapper
                         ============================================= */
                      .email-wrapper {
                        width: 100%;
                        background-color: #eef1f6;
                        padding: 32px 16px;
                      }
                      .email-container {
                        max-width: 620px;
                        margin: 0 auto;
                        background-color: #ffffff;
                        border-radius: 16px;
                        overflow: hidden;
                        box-shadow: 0 4px 24px rgba(26, 39, 68, 0.10);
                      }

                      /* =============================================
                         Header
                         ============================================= */
                      .email-header {
                        background: linear-gradient(135deg, #1a2744 0%, #253460 60%, #1a2744 100%);
                        padding: 32px 40px 28px;
                        text-align: center;
                      }
                      .brand-logo {
                        display: inline-flex;
                        align-items: center;
                        gap: 10px;
                        text-decoration: none;
                      }
                      .brand-icon {
                        width: 44px;
                        height: 44px;
                        background: linear-gradient(135deg, #e53935, #c62828);
                        border-radius: 10px;
                        display: inline-flex;
                        align-items: center;
                        justify-content: center;
                        font-size: 22px;
                        line-height: 44px;
                        text-align: center;
                        vertical-align: middle;
                      }
                      .brand-name {
                        font-size: 26px;
                        font-weight: 800;
                        color: #ffffff;
                        letter-spacing: 0.5px;
                        vertical-align: middle;
                      }
                      .brand-name span {
                        color: #ef5350;
                      }
                      .header-tagline {
                        color: #8da3c7;
                        font-size: 12px;
                        margin-top: 6px;
                        letter-spacing: 0.4px;
                      }

                      /* =============================================
                         Success Banner
                         ============================================= */
                      .success-banner {
                        background: linear-gradient(160deg, #f0fdf4 0%, #dcfce7 100%);
                        border-bottom: 3px solid #22c55e;
                        padding: 36px 40px 32px;
                        text-align: center;
                      }
                      .success-icon-wrap {
                        width: 80px;
                        height: 80px;
                        background: linear-gradient(135deg, #22c55e, #16a34a);
                        border-radius: 50%;
                        margin: 0 auto 18px;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        box-shadow: 0 8px 24px rgba(34, 197, 94, 0.30);
                        font-size: 38px;
                        line-height: 80px;
                        text-align: center;
                      }
                      .success-title {
                        font-size: 26px;
                        font-weight: 800;
                        color: #15803d;
                        margin-bottom: 8px;
                      }
                      .success-subtitle {
                        font-size: 14px;
                        color: #4b7c5e;
                        line-height: 1.6;
                        max-width: 460px;
                        margin: 0 auto;
                      }

                      /* =============================================
                         Order Code Badge
                         ============================================= */
                      .order-code-section {
                        padding: 24px 40px 0;
                        text-align: center;
                      }
                      .order-code-badge {
                        display: inline-block;
                        background: #f0f4ff;
                        border: 2px dashed #1a2744;
                        border-radius: 12px;
                        padding: 14px 32px;
                      }
                      .order-code-label {
                        font-size: 11px;
                        text-transform: uppercase;
                        letter-spacing: 1.2px;
                        color: #6b7fa8;
                        margin-bottom: 4px;
                      }
                      .order-code-value {
                        font-size: 24px;
                        font-weight: 800;
                        color: #1a2744;
                        letter-spacing: 1px;
                      }
                      .order-code-value span {
                        color: #e53935;
                      }
                      .order-date {
                        font-size: 12px;
                        color: #8da3c7;
                        margin-top: 4px;
                      }

                      /* =============================================
                         Section Styles
                         ============================================= */
                      .section {
                        padding: 24px 40px;
                      }
                      .section + .section {
                        padding-top: 0;
                      }
                      .section-title {
                        font-size: 13px;
                        font-weight: 700;
                        text-transform: uppercase;
                        letter-spacing: 1px;
                        color: #6b7fa8;
                        margin-bottom: 14px;
                        display: flex;
                        align-items: center;
                        gap: 6px;
                      }
                      .section-title::after {
                        content: '';
                        flex: 1;
                        height: 1px;
                        background: #e8edf5;
                        display: inline-block;
                        vertical-align: middle;
                      }
                      .divider {
                        height: 1px;
                        background-color: #e8edf5;
                        margin: 0 40px;
                      }

                      /* =============================================
                         Customer Info
                         ============================================= */
                      .info-grid {
                        display: grid;
                        grid-template-columns: 1fr 1fr;
                        gap: 12px;
                      }
                      .info-item {
                        background: #f8faff;
                        border: 1px solid #e8edf5;
                        border-radius: 10px;
                        padding: 12px 14px;
                      }
                      .info-item-label {
                        font-size: 11px;
                        color: #8da3c7;
                        text-transform: uppercase;
                        letter-spacing: 0.8px;
                        margin-bottom: 3px;
                      }
                      .info-item-value {
                        font-size: 14px;
                        font-weight: 600;
                        color: #1e2a45;
                      }
                      .info-item.full-width {
                        grid-column: 1 / -1;
                      }

                      /* =============================================
                         Order Items Table
                         ============================================= */
                      .items-table {
                        width: 100%;
                        border-collapse: collapse;
                        font-size: 14px;
                      }
                      .items-table thead tr {
                        background: #1a2744;
                        color: #ffffff;
                      }
                      .items-table thead th {
                        padding: 10px 12px;
                        text-align: left;
                        font-size: 11px;
                        font-weight: 600;
                        text-transform: uppercase;
                        letter-spacing: 0.8px;
                      }
                      .items-table thead th:last-child {
                        text-align: right;
                      }
                      .items-table tbody tr {
                        border-bottom: 1px solid #e8edf5;
                      }
                      .items-table tbody tr:last-child {
                        border-bottom: none;
                      }
                      .items-table tbody tr:hover {
                        background: #f8faff;
                      }
                      .items-table tbody td {
                        padding: 14px 12px;
                        vertical-align: middle;
                      }
                      .item-thumb {
                        width: 52px;
                        height: 52px;
                        object-fit: cover;
                        border-radius: 8px;
                        background: #eef1f6;
                        border: 1px solid #e8edf5;
                      }
                      .item-thumb-placeholder {
                        width: 52px;
                        height: 52px;
                        border-radius: 8px;
                        background: linear-gradient(135deg, #e8edf5, #d0d8ed);
                        display: inline-flex;
                        align-items: center;
                        justify-content: center;
                        font-size: 22px;
                        vertical-align: middle;
                        line-height: 52px;
                        text-align: center;
                      }
                      .item-info {
                        padding-left: 12px;
                      }
                      .item-name {
                        font-weight: 600;
                        color: #1e2a45;
                        margin-bottom: 2px;
                      }
                      .item-variant {
                        font-size: 12px;
                        color: #8da3c7;
                      }
                      .item-sku {
                        font-size: 11px;
                        color: #b0bcd4;
                        font-family: 'Courier New', monospace;
                      }
                      .item-qty {
                        text-align: center;
                        font-weight: 600;
                        color: #4b5d82;
                      }
                      .item-price {
                        text-align: right;
                        font-weight: 600;
                        color: #1e2a45;
                        white-space: nowrap;
                      }

                      /* =============================================
                         Pricing Summary
                         ============================================= */
                      .pricing-box {
                        background: #f8faff;
                        border: 1px solid #e8edf5;
                        border-radius: 12px;
                        padding: 16px 20px;
                      }
                      .pricing-row {
                        display: flex;
                        justify-content: space-between;
                        align-items: center;
                        padding: 6px 0;
                        font-size: 14px;
                        color: #4b5d82;
                      }
                      .pricing-row + .pricing-row {
                        border-top: 1px dashed #e8edf5;
                      }
                      .pricing-row.discount {
                        color: #16a34a;
                        font-weight: 600;
                      }
                      .pricing-row.total {
                        border-top: 2px solid #1a2744 !important;
                        margin-top: 4px;
                        padding-top: 12px;
                        font-size: 17px;
                        font-weight: 800;
                        color: #1a2744;
                      }
                      .pricing-row.total .total-amount {
                        color: #e53935;
                        font-size: 20px;
                      }

                      /* =============================================
                         Payment Badge
                         ============================================= */
                      .payment-section {
                        display: flex;
                        gap: 12px;
                        flex-wrap: wrap;
                      }
                      .payment-badge {
                        display: inline-flex;
                        align-items: center;
                        gap: 8px;
                        background: #f0f4ff;
                        border: 1.5px solid #c7d2eb;
                        border-radius: 10px;
                        padding: 10px 18px;
                      }
                      .payment-badge-icon {
                        font-size: 20px;
                        line-height: 1;
                      }
                      .payment-badge-text {
                        font-size: 13px;
                      }
                      .payment-badge-label {
                        display: block;
                        font-size: 10px;
                        text-transform: uppercase;
                        letter-spacing: 0.8px;
                        color: #8da3c7;
                      }
                      .payment-badge-value {
                        display: block;
                        font-weight: 700;
                        color: #1e2a45;
                      }
                      .status-paid {
                        background: #f0fdf4;
                        border-color: #86efac;
                      }
                      .status-paid .payment-badge-value {
                        color: #16a34a;
                      }
                      .status-pending {
                        background: #fffbeb;
                        border-color: #fcd34d;
                      }
                      .status-pending .payment-badge-value {
                        color: #d97706;
                      }

                      /* =============================================
                         CTA Button
                         ============================================= */
                      .cta-section {
                        padding: 8px 40px 32px;
                        text-align: center;
                      }
                      .cta-button {
                        display: inline-block;
                        background: linear-gradient(135deg, #e53935, #c62828);
                        color: #ffffff !important;
                        text-decoration: none;
                        font-size: 15px;
                        font-weight: 700;
                        padding: 14px 40px;
                        border-radius: 12px;
                        letter-spacing: 0.3px;
                        box-shadow: 0 4px 16px rgba(229, 57, 53, 0.35);
                        margin-bottom: 12px;
                        transition: opacity 0.2s;
                      }
                      .cta-button:hover {
                        opacity: 0.90;
                      }
                      .cta-secondary {
                        display: inline-block;
                        margin-left: 12px;
                        background: #f0f4ff;
                        color: #1a2744 !important;
                        text-decoration: none;
                        font-size: 14px;
                        font-weight: 600;
                        padding: 14px 28px;
                        border-radius: 12px;
                        border: 1.5px solid #c7d2eb;
                      }

                      /* =============================================
                         Help Note
                         ============================================= */
                      .help-note {
                        background: #fff8f0;
                        border-left: 4px solid #f59e0b;
                        padding: 14px 18px;
                        border-radius: 0 10px 10px 0;
                        font-size: 13px;
                        color: #7c5c2e;
                        line-height: 1.5;
                        margin: 0 40px 24px;
                      }
                      .help-note strong {
                        color: #92400e;
                      }

                      /* =============================================
                         Footer
                         ============================================= */
                      .email-footer {
                        background: #1a2744;
                        padding: 30px 40px;
                        text-align: center;
                        color: #8da3c7;
                      }
                      .footer-brand {
                        font-size: 18px;
                        font-weight: 800;
                        color: #ffffff;
                        margin-bottom: 4px;
                      }
                      .footer-brand span {
                        color: #ef5350;
                      }
                      .footer-desc {
                        font-size: 12px;
                        color: #6b7fa8;
                        margin-bottom: 18px;
                      }
                      .social-links {
                        margin-bottom: 20px;
                      }
                      .social-link {
                        display: inline-block;
                        margin: 0 6px;
                        width: 34px;
                        height: 34px;
                        background: rgba(255, 255, 255, 0.08);
                        border-radius: 8px;
                        text-align: center;
                        line-height: 34px;
                        text-decoration: none;
                        font-size: 16px;
                        transition: background 0.2s;
                        vertical-align: middle;
                      }
                      .social-link:hover {
                        background: rgba(255, 255, 255, 0.15);
                      }
                      .footer-links {
                        margin-bottom: 16px;
                      }
                      .footer-link {
                        color: #8da3c7;
                        text-decoration: none;
                        font-size: 12px;
                        margin: 0 8px;
                      }
                      .footer-link:hover {
                        color: #ffffff;
                      }
                      .footer-contact {
                        font-size: 12px;
                        color: #6b7fa8;
                        margin-bottom: 16px;
                        line-height: 1.8;
                      }
                      .footer-legal {
                        font-size: 11px;
                        color: #4b5d82;
                        line-height: 1.6;
                      }
                      .unsubscribe-link {
                        color: #6b7fa8;
                        text-decoration: underline;
                      }

                      /* =============================================
                         Responsive
                         ============================================= */
                      @media only screen and (max-width: 620px) {
                        .email-wrapper {
                          padding: 16px 8px;
                        }
                        .email-header,
                        .success-banner,
                        .section,
                        .cta-section,
                        .help-note,
                        .email-footer {
                          padding-left: 20px !important;
                          padding-right: 20px !important;
                        }
                        .divider {
                          margin: 0 20px;
                        }
                        .help-note {
                          margin-left: 20px;
                          margin-right: 20px;
                        }
                        .info-grid {
                          grid-template-columns: 1fr;
                        }
                        .info-item.full-width {
                          grid-column: 1;
                        }
                        .cta-button,
                        .cta-secondary {
                          display: block;
                          margin: 8px 0;
                        }
                        .brand-name {
                          font-size: 22px;
                        }
                        .success-title {
                          font-size: 22px;
                        }
                        .items-table thead th:nth-child(2),
                        .items-table tbody td:nth-child(2) {
                          display: none;
                        }
                      }
                    </style>
                  </head>
                  <body>
                    <div class="email-wrapper">
                      <div class="email-container">

                        <!-- ===== HEADER ===== -->
                        <div class="email-header">
                          <div class="brand-logo">
                            <span class="brand-icon">🧩</span>
                            <span class="brand-name">Puz<span>Kit</span>3D</span>
                          </div>
                          <p class="header-tagline">Mô hình Lắp ráp Trí tuệ 3D &amp; Thiết kế theo yêu cầu</p>
                        </div>

                        <!-- ===== SUCCESS BANNER ===== -->
                        <div class="success-banner">
                          <div class="success-icon-wrap">✓</div>
                          <h1 class="success-title">Đặt hàng thành công!</h1>
                          <p class="success-subtitle">
                            Xin chào <strong>{{CUSTOMER_NAME}}</strong>, đơn hàng của bạn đã được xác nhận.
                            Chúng tôi sẽ xử lý và giao hàng đến bạn sớm nhất có thể.
                          </p>
                        </div>

                        <!-- ===== ORDER CODE ===== -->
                        <div class="order-code-section">
                          <div class="order-code-badge">
                            <p class="order-code-label">📦 Mã đơn hàng</p>
                            <p class="order-code-value"><span>#</span>{{ORDER_CODE}}</p>
                            <p class="order-date">Ngày đặt: {{ORDER_DATE}}</p>
                          </div>
                        </div>

                        <!-- ===== DIVIDER ===== -->
                        <div class="section" style="padding-bottom: 0;">
                          <p class="section-title">🙋 Thông tin khách hàng</p>
                        </div>

                        <!-- ===== CUSTOMER INFO ===== -->
                        <div class="section" style="padding-top: 0;">
                          <div class="info-grid">
                            <div class="info-item">
                              <p class="info-item-label">Họ và tên</p>
                              <p class="info-item-value">{{CUSTOMER_NAME}}</p>
                            </div>
                            <div class="info-item">
                              <p class="info-item-label">Số điện thoại</p>
                              <p class="info-item-value">{{CUSTOMER_PHONE}}</p>
                            </div>
                            <div class="info-item">
                              <p class="info-item-label">Email</p>
                              <p class="info-item-value">{{CUSTOMER_EMAIL}}</p>
                            </div>
                            <div class="info-item">
                              <p class="info-item-label">Tỉnh / Thành phố</p>
                              <p class="info-item-value">{{SHIPPING_PROVINCE}}</p>
                            </div>
                            <div class="info-item full-width">
                              <p class="info-item-label">Địa chỉ giao hàng</p>
                              <p class="info-item-value">{{SHIPPING_WARD}}, {{SHIPPING_DISTRICT}}, {{SHIPPING_PROVINCE}}</p>
                            </div>
                          </div>
                        </div>

                        <div class="divider"></div>

                        <!-- ===== ORDER ITEMS ===== -->
                        <div class="section">
                          <p class="section-title">🛒 Sản phẩm đã đặt</p>
                          <table class="items-table">
                            <thead>
                              <tr>
                                <th style="border-radius: 8px 0 0 8px;" colspan="2">Sản phẩm</th>
                                <th style="text-align:center;">SL</th>
                                <th style="text-align:right; border-radius: 0 8px 8px 0;">Thành tiền</th>
                              </tr>
                            </thead>
                            <tbody>
                              <!--
                                ========================================================
                                REPEAT THIS BLOCK FOR EACH ORDER ITEM
                                ========================================================
                              -->
                              <tr>
                                <!-- Item thumbnail + info -->
                                <td style="width: 64px; vertical-align: top; padding-right: 0;">
                                  <!-- Option A: product image -->
                                  <!-- <img src="{{ITEM_IMAGE_URL}}" alt="{{ITEM_NAME}}" class="item-thumb" width="52" height="52"> -->
                                  <!-- Option B: emoji placeholder (remove when using real image) -->
                                  <div class="item-thumb-placeholder">🧩</div>
                                </td>
                                <td class="item-info">
                                  <p class="item-name">{{ITEM_PRODUCT_NAME}}</p>
                                  <p class="item-variant">Phân loại: {{ITEM_VARIANT_NAME}}</p>
                                  <p class="item-sku">SKU: {{ITEM_SKU}}</p>
                                </td>
                                <td class="item-qty">{{ITEM_QUANTITY}}</td>
                                <td class="item-price">{{ITEM_TOTAL_AMOUNT}}</td>
                              </tr>
                              <!--
                                ========================================================
                                END REPEAT BLOCK
                                ========================================================
                              -->

                              <!-- Example second item (remove in production) -->
                              <tr>
                                <td style="width: 64px; vertical-align: top; padding-right: 0;">
                                  <div class="item-thumb-placeholder">🎯</div>
                                </td>
                                <td class="item-info">
                                  <p class="item-name">{{ITEM_PRODUCT_NAME_2}}</p>
                                  <p class="item-variant">Phân loại: {{ITEM_VARIANT_NAME_2}}</p>
                                  <p class="item-sku">SKU: {{ITEM_SKU_2}}</p>
                                </td>
                                <td class="item-qty">{{ITEM_QUANTITY_2}}</td>
                                <td class="item-price">{{ITEM_TOTAL_AMOUNT_2}}</td>
                              </tr>
                            </tbody>
                          </table>
                        </div>

                        <div class="divider"></div>

                        <!-- ===== PRICING SUMMARY ===== -->
                        <div class="section">
                          <p class="section-title">💰 Tóm tắt thanh toán</p>
                          <div class="pricing-box">
                            <div class="pricing-row">
                              <span>Tạm tính</span>
                              <span>{{SUB_TOTAL_AMOUNT}}</span>
                            </div>
                            <div class="pricing-row">
                              <span>Phí vận chuyển</span>
                              <span>{{SHIPPING_FEE}}</span>
                            </div>
                            <!-- Show this row only when coin discount > 0 -->
                            <div class="pricing-row discount">
                              <span>🪙 Giảm từ PuzCoin</span>
                              <span>- {{USED_COIN_AS_MONEY}}</span>
                            </div>
                            <div class="pricing-row total">
                              <span>Tổng cộng</span>
                              <span class="total-amount">{{GRAND_TOTAL_AMOUNT}}</span>
                            </div>
                          </div>
                        </div>

                        <div class="divider"></div>

                        <!-- ===== PAYMENT METHOD ===== -->
                        <div class="section">
                          <p class="section-title">💳 Phương thức thanh toán</p>
                          <div class="payment-section">
                            <div class="payment-badge">
                              <span class="payment-badge-text">
                                <span class="payment-badge-label">Phương thức</span>
                                <span class="payment-badge-value">{{PAYMENT_METHOD_LABEL}}</span>
                              </span>
                            </div>
                          </div>
                          <!--
                            Nếu ONLINE:
                              Hiển thị nút thanh toán ngay và hạn chót
                            Nếu COD:
                              Hiển thị nút theo dõi đơn hàng
                          -->
                          <div class="cta-section">
                            <!-- ONLINE -->
                            {{#if PAYMENT_METHOD_ONLINE}}
                              <a href="{{PAYMENT_URL}}" class="cta-button">Thanh toán ngay</a>
                              <p style="font-size:13px;color:#d97706;margin-top:8px;">Hạn chót thanh toán: <strong>{{PAYMENT_DEADLINE}}</strong></p>
                            {{/if}}
                            <!-- COD -->
                            {{#if PAYMENT_METHOD_COD}}
                              <a href="{{ORDER_DETAIL_URL}}" class="cta-secondary">Xem chi tiết đơn hàng</a>
                            {{/if}}
                          </div>
                        </div>

                        <div class="divider"></div>

                        <!-- ===== CTA BUTTONS ===== -->
                        <div class="cta-section">
                          <a href="{{ORDER_DETAIL_URL}}" class="cta-button">Xem chi tiết đơn hàng →</a>
                          <a href="{{SHOP_URL}}" class="cta-secondary">Khám phá thêm</a>
                        </div>

                        <!-- ===== HELP NOTE ===== -->
                        <div class="help-note">
                          <strong>Cần hỗ trợ?</strong> Nếu có bất kỳ thắc mắc nào về đơn hàng
                          <strong>{{ORDER_CODE}}</strong>, vui lòng liên hệ với chúng tôi qua email
                          <a href="mailto:contact@puzkit3d.com" style="color: #92400e;">contact@puzkit3d.com</a>
                          hoặc hotline <strong>1900-xxxx</strong> trong giờ làm việc (Thứ 2 – Thứ 7: 8:00 – 18:00).
                        </div>

                        <!-- ===== FOOTER ===== -->
                        <div class="email-footer">
                          <p class="footer-brand">Puz<span>Kit</span>3D</p>
                          <p class="footer-desc">Mô hình Lắp ráp Trí tuệ 3D &amp; Thiết kế theo yêu cầu</p>

                          <div class="social-links">
                            <a href="https://facebook.com/puzkit3d" class="social-link" title="Facebook">f</a>
                            <a href="https://instagram.com/puzkit3d" class="social-link" title="Instagram">ig</a>
                            <a href="https://youtube.com/@puzkit3d" class="social-link" title="YouTube">yt</a>
                            <a href="https://tiktok.com/@puzkit3d" class="social-link" title="TikTok">tt</a>
                          </div>

                          <div class="footer-links">
                            <a href="{{SHOP_URL}}" class="footer-link">Cửa hàng</a>
                            <a href="{{ORDERS_URL}}" class="footer-link">Đơn hàng của tôi</a>
                            <a href="{{PROFILE_URL}}" class="footer-link">Tài khoản</a>
                            <a href="mailto:contact@puzkit3d.com" class="footer-link">Liên hệ</a>
                          </div>

                          <div class="footer-contact">
                            📍 TP. Hồ Chí Minh, Việt Nam &nbsp;|&nbsp;
                            📞 1900-xxxx &nbsp;|&nbsp;
                            ✉️ contact@puzkit3d.com
                          </div>

                          <p class="footer-legal">
                            © 2026 PuzKit3D. Tất cả quyền được bảo lưu.<br />
                            Bạn nhận được email này vì đã đặt hàng tại PuzKit3D.<br />
                            <a href="{{UNSUBSCRIBE_URL}}" class="unsubscribe-link">Huỷ đăng ký nhận email</a>
                          </p>
                        </div>

                      </div><!-- /.email-container -->
                    </div><!-- /.email-wrapper -->
                  </body>
                </html>
                
                """
            }
        };

        await _ses.DeleteTemplateAsync(new DeleteTemplateRequest
        {
            TemplateName = EmailTemplate.OrderCreatedSuccessEmailTemplate
        });
        
        await _ses.CreateTemplateAsync(orderCreatedSuccessEmailTemplate);
    }
}
