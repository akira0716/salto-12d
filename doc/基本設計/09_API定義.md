# API定義書

フロントエンドとバックエンドの連携に用いる主要な RESTful API エンドポイントの定義です。

## 1. 共通要件
*   **Base URL**: `/api/v1`
*   **データ形式**: `application/json`
*   **認証方式**: JWT等のトークンベース認証。ログイン後に発行されるトークンを `Authorization: Bearer <token>` ヘッダに付与する。

---

## 2. エンドポイント一覧

| メソッド | エンドポイント | 説明 | 権限 |
| :--- | :--- | :--- | :--- |
| **POST** | `/auth/login` | ログイン（認証トークンの取得） | 未認証 |
| **POST** | `/auth/signup` | サインアップ（ユーザー登録） | 未認証 |
| **GET** | `/equipments` | 備品一覧取得・検索（一般社員は「利用可」のみ） | 共通 |
| **GET** | `/equipments/:id` | 備品の個別情報取得 | 共通 |
| **POST** | `/equipments` | 新規備品（個体）の登録 | 管理者 |
| **PUT** | `/equipments/:id` | 備品情報の更新（ステータス強制変更など） | 管理者 |
| **GET** | `/categories` | 備品分類（カテゴリ）一覧の取得 | 共通 |
| **POST** | `/categories` | 備品分類（カテゴリ）の追加 | 管理者 |
| **PUT** | `/categories/:id` | 備品分類（カテゴリ）の編集 | 管理者 |
| **DELETE**| `/categories/:id` | 備品分類（カテゴリ）の削除 | 管理者 |
| **POST** | `/loan-requests` | 備品の貸出申請を行う | 一般社員 |
| **GET** | `/loan-requests/me` | 自身の貸出・申請履歴を取得（マイページ用） | 一般社員 |
| **GET** | `/admin/loan-requests` | 貸出申請一覧の取得 | 管理者 |
| **GET** | `/admin/loans` | 全社の貸出状況の取得（遅延絞り込み含む） | 管理者 |
| **PATCH**| `/admin/loan-requests/:id/approve` | 貸出申請の承認 | 管理者 |
| **PATCH**| `/admin/loan-requests/:id/reject` | 貸出申請の却下（却下理由の入力） | 管理者 |
| **PATCH**| `/admin/loans/:id/return` | 備品の返却受領処理 | 管理者 |

---

## 3. API詳細（主要機能）

### 3.1 備品一覧・検索
*   **GET** `/equipments`
*   **クエリパラメータ**:
    *   `category_id` (数値): カテゴリ絞り込み
    *   `keyword` (文字): 備品名によるあいまい検索
    *   `status` (文字): ステータス絞り込み（一般利用者はデフォルトで `available` を送信するかバックエンドで固定）
*   **レスポンス**:
    ```json
    {
      "equipments": [
        {
          "id": 1001,
          "name": "ノートPC A",
          "category": { "id": 1, "name": "PC本体" },
          "status": "available",
          "description": "..."
        }
      ]
    }
    ```

### 3.2 貸出申請
*   **POST** `/loan-requests`
*   **リクエスト**:
    ```json
    {
      "equipment_id": 1001,
      "start_date": "2026-04-05",
      "end_date": "2026-04-10",
      "purpose": "出張時のプレゼン用として使用するため"
    }
    ```
*   **レスポンス**: `201 Created`

### 3.3 貸出申請の却下（異常系）
*   **PATCH** `/admin/loan-requests/:id/reject`
*   **リクエスト**:
    ```json
    {
      "rejection_reason": "貸出直前の点検で故障が発覚したため。",
      "set_equipment_broken": true 
      // trueにすると対象備品のステータスも同時に「修理中」へ変更
    }
    ```
*   **レスポンス**: `200 OK`

### 3.4 備品の返却受領
*   **PATCH** `/admin/loans/:id/return`
*   **説明**: 管理者が物理的な備品を受け取った際に実行。
    *   `LOANS.status` を `返却済` に更新し、`return_date` を記録。
    *   `EQUIPMENTS.status` を `利用可` に戻す。
*   **リクエスト**: ボディなし（パスパラメータのIDのみ）
*   **レスポンス**: `200 OK`
